using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Conduit
{
    public partial class CheckProgress : Form
    {
        private MainWindow m = new MainWindow();
        public CheckProgress(MainWindow v)
        {
            InitializeComponent();
            m = v;
            /*string file = File.ReadAllText("C:/Users/kbowers/Desktop/test5.txt");
            fixOutput(file);*/
            if (runCommand() != null) {
                fixOutput(runCommand());
            }
            
        }
        public string runCommand()
        {
            SshClient ssh = new SshClient("headnode.beocat.ksu.edu", m.user, m.password);

            if (ssh != null)
            {
                using (ssh)
                {
                    ssh.Connect();
                    var command = ssh.CreateCommand("kstat --me");
                    var results = command.Execute();

                    return results;

                }
            }
            else
            {
                return null;
            }

        }
        public void fixOutput(string results)
        {
            string[] lines = results.Split('\n');
            int count = lines.Count();

            string name = "";
            /*string[] nodeName = new string[count];
            string[] jobName = new string[count];
            string[] jobID = new string[count];
            string[] cores = new string[count];
            string[] maxGB = new string[count];
            string[] runTime = new string[count];*/
            Dictionary<string, int> NodeNames = new Dictionary<string, int>();
            List<string> NodeName = new List<string>();
            List<string> JobNames = new List<string>();
            List<string> JobID = new List<string>();
            List<string> Cores = new List<string>();
            List<string> MaxGB = new List<string>();
            List<string> RunTime = new List<string>();
            List<string> FutureJobNames = new List<string>();
            List<string> FutureJobID = new List<string>();
            List<string> FutureCores = new List<string>();
            bool isQueued = false;
            bool containsMax = false;

            //int nodes = 0;
            //int b = 0;
            for (int i = 0; i < count; i++)
            {
                //if line is empty skip it
                if (lines[i] == "")
                {
                }
                else
                {
                    string[] values = lines[i].Split();
                    string[] nonNullValues = new string[values.Count()];
                    //MessageBox.Show(values[i]);
                    char[] characters = lines[i].ToCharArray();
                    if (characters.Count() > 0)
                    {
                        //NodeName
                        if (characters[0] >= 65 && characters[0] <= 90)
                        {
                            //MessageBox.Show(characters[0].ToString());
                            for (int j = 0; j < characters.Count(); j++)
                            {
                                if(characters[j] !=27)
                                {
                                    name += characters[j];
                                }
                                else
                                {
                                    break;
                                }
                                
                            }
                            NodeNames.Add(name, i);
                        }
                        //job we want
                        else if (characters[0] == 32 && characters[1] == 27)
                        {
                            int k = 0;
                            for(int j = 0; j < values.Count(); j++)
                            {
                               
                                if (values[j] == "")
                                {
                                }
                                else
                                {
                                    nonNullValues[k] = values[j];
                                    k++;
                                }
                            }
                            if (isQueued)
                            {
                                if (nonNullValues.Count() > 1)
                                {
                                    char[] future = nonNullValues[1].ToCharArray();
                                    //contains queued job
                                    if (future[0] == 27)
                                    {
                                        FutureJobNames.Add(nonNullValues[2]);
                                        FutureJobID.Add(nonNullValues[3]);
                                        FutureCores.Add(nonNullValues[5]);

                                    }
                                }
                            }
                            else
                            {
                                //Assumption that only your jobs start with 32,27
                                JobNames.Add(nonNullValues[2]);
                                JobID.Add(nonNullValues[3]);
                                Cores.Add(nonNullValues[5]);
                                char[] gb = nonNullValues[8].ToCharArray();
                                string max = "";
                                for (int l = 4; l < gb.Count(); l++)
                                {
                                    max += gb[l];
                                }
                                if (max.Last() == 'x')
                                {
                                    max +="" + nonNullValues[9];
                                    containsMax = true;
                                }
                                else
                                {
                                    containsMax = false;
                                }
                                MaxGB.Add(max);
                                string run = "";
                                if (containsMax)
                                {
                                    char[] rt = nonNullValues[17].ToCharArray();
                                    run = nonNullValues[12] + nonNullValues[13] + " " + nonNullValues[14] + nonNullValues[15] + " " + nonNullValues[16] + rt[0];
                                }
                                else
                                {
                                    char[] rt = nonNullValues[16].ToCharArray();
                                    run = nonNullValues[11] + nonNullValues[12] + " " + nonNullValues[13] + nonNullValues[14] + " " + nonNullValues[15] + rt[0];
                                }
                                /*char[] rt = nonNullValues[17].ToCharArray();
                                string run = nonNullValues[12] + nonNullValues[13] + " " + nonNullValues[14] + nonNullValues[15] + " " + nonNullValues[16] + rt[0];
                                RunTime.Add(run);*/
                                RunTime.Add(run);
                            }
                        }
                        //ignore other people
                        else if(characters[0] == 32 && characters[1] != 27)
                        {

                        }
                        //Queue lines
                        else if(characters[0] == 27)
                        {
                            isQueued = true;
                            int k = 0;
                            for (int j = 0; j < values.Count(); j++)
                            {

                                if (values[j] == "")
                                {
                                }
                                else
                                {
                                    nonNullValues[k] = values[j];
                                    k++;
                                }
                            }
                            
                            if (nonNullValues.Count() > 1)
                            {
                               char[] future = nonNullValues[1].ToCharArray();
                                //contains queued job
                                if (future[0] == 27)
                                {
                                    FutureJobNames.Add(nonNullValues[2]);
                                    FutureJobID.Add(nonNullValues[3]);
                                    FutureCores.Add(nonNullValues[5]);

                                }
                                else
                                { 
                                }
                            }
                        }
                    }
                }
            }
            
                for (int i = 0; i < JobNames.Count; i++)
                {
                    NodeName.Add(name);
                }
            
           
                foreach (var item in NodeNames)
                {

                    string theName = item.Key;
                    int times = item.Value;
                    for (int i = times; i < JobNames.Count; i ++)
                    {
                        NodeName[i] = theName;
                    }
                }
            
            
            string[] nodeName = NodeName.ToArray();
            string[] jobName = JobNames.ToArray();
            MessageBox.Show(jobName.Count().ToString());
            string[] jobID = JobID.ToArray();
            string[] cores = Cores.ToArray();
            string[] maxGB = MaxGB.ToArray();
            string[] runTime = RunTime.ToArray();
            string[] futurejobname= FutureJobNames.ToArray();
            string[] futurejobid = FutureJobID.ToArray();
            string[] futurecores= FutureCores.ToArray();

            String s = String.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} \r\n\r\n", "Node", "Job Name", "Job ID", "Cores", "Max gb", "Run Time");
            for (int index = 0; index < jobName.Length; index++)
                s += String.Format("{0,-10} {1,-10}{2,-10} {3,-10} {4,-10} {5,-10}\r\n",
                                   nodeName[index], jobName[index], jobID[index], cores[index], maxGB[index], runTime[index]);
            kstatProgress.AppendText($"\r\n{s}");

            kstatProgress.AppendText("\r\n\r\n");
            kstatProgress.AppendText("#################### Beocat Queue ####################\r\n");
            String r = String.Format("{0,-10} {1,-10} {2,-10} \r\n\r\n","Job Name", "Job ID", "Cores");
            for (int index = 0; index < futurejobname.Length; index++)
                r += String.Format("{0,-10} {1,-10}{2,-10}\r\n",
                                   futurejobname[index], futurejobid[index], futurecores[index]);
            kstatProgress.AppendText($"\r\n{r}");
            

        }

    }
}
