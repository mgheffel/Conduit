using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conduit
{
    class ScriptCreator
    {
        string dataPath;
        string masterSkeletonPath;
        string parallelSkeletonPath;
        string pipelinePath;
        string parentDirectory;
        public String paramTups = "";
        public String inputTups = "";
        private static Dictionary<string, string> convertSbatch = new Dictionary<string, string>()
        {
            { "#runTime", "time" },
            { "#numCPUs", "ntasks-per-node"},
            { "#memPerCPU", "mem-per-cpu"}
        };

        public ScriptCreator(Node n, string inputString, string conduitPath, string pipePath, string parentDir)
        {
            pipelinePath = pipePath;
            parentDirectory = parentDir;
            dataPath = conduitPath + "\\data";
            masterSkeletonPath = dataPath + "\\skeletons\\" + n.Name + "M.txt";
            parallelSkeletonPath = dataPath + "\\skeletons\\" + n.Name + "P.txt";
            if (!File.Exists(parallelSkeletonPath))
            {
                parallelSkeletonPath = "";
            }
            inputTups = inputString;
            if (n.V1 != "")
                paramTups += n.V1 + ',' + n.T1 + ';';
            if (n.V2 != "")
                paramTups += n.V2 + ',' + n.T2 + ';';
            if (n.V3 != "")
                paramTups += n.V3 + ',' + n.T3 + ';';
            if (n.V4 != "")
                paramTups += n.V4 + ',' + n.T4 + ';';
            if (n.V5 != "")
                paramTups += n.V5 + ',' + n.T5 + ';';
            if (n.V6 != "")
                paramTups += n.V6 + ',' + n.T6 + ';';
            if (n.V7 != "")
                paramTups += n.V7 + ',' + n.T7 + ';';
            if (n.V8 != "")
                paramTups += n.V8 + ',' + n.T8 + ';';
            if (n.V9 != "")
                paramTups += n.V9 + ',' + n.T9 + ';';
            if (n.V10 != "")
                paramTups += n.V10 + ',' + n.T10 + ';';
            paramTups = paramTups.Substring(0, paramTups.Length - 1);
        }

        public void compileScripts()
        {
            if (File.Exists(parallelSkeletonPath))
            {
                //compileMasterScript();
                //compileParallelScript();
            }
        }

        public void compileMasterScript(string path, string outDirs)
        {
            string baseFileText = System.IO.File.ReadAllText(masterSkeletonPath);
            baseFileText = baseFileText.Replace("*&%@pipelinePathTag", pipelinePath);
            baseFileText = baseFileText.Replace("*&%@parentDirTag", parentDirectory);
            string[] inputs = inputTups.Split(';');
            for (int i = 0; i < inputs.Length; i++)
            {
                string[] split = inputs[i].Split(',');
                baseFileText = baseFileText.Replace("*&%@" + split[0] + "Tag", split[1]);
            }
            string[] outputs = outDirs.Split(';');
            for (int i = 0; i < outputs.Length; i++)
            {
                string[] split = outputs[i].Split(',');
                baseFileText = baseFileText.Replace("*&%@" + split[0] + "Tag", split[1]);
            }
            File.WriteAllText(path, baseFileText.Replace("\r\n", "\n"));
        }



        public void compileParallelScript(string path)
        {
            string baseFileText = System.IO.File.ReadAllText(parallelSkeletonPath);
            string[] pars = paramTups.Split(';');
            List<string> sbatchParams = new List<string>();
            List<string> softwareParams = new List<string>();
            for (int i = 0; i < pars.Length; i++)
            {
                if (pars[i].Substring(0, 1) == "#")
                {
                    sbatchParams.Add(pars[i]);
                }
                else
                {
                    softwareParams.Add(pars[i]);
                }
            }
            string[] fileSplit = baseFileText.Split(new String[] { "*&%@pTag" }, StringSplitOptions.None);
            string parallelFileText = "";
            for (int i = 0; i < fileSplit.Length - 1; i++)
            {
                string[] inputSplit = softwareParams[i].Split(',');
                parallelFileText += fileSplit[i] + inputSplit[0] + '=' + inputSplit[1];
            }
            parallelFileText += fileSplit[fileSplit.Length - 1];
            fileSplit = parallelFileText.Split(new String[] { "*&%@sTag" }, StringSplitOptions.None);
            parallelFileText = fileSplit[0];
            for (int i = 0; i < sbatchParams.Count; i++)
            {
                string[] inputSplit = sbatchParams[i].Split(',');
                parallelFileText += "#SBATCH --" + convertSbatch[inputSplit[0]] + '=' + inputSplit[1] + '\n';
            }
            parallelFileText += fileSplit[1];
            File.WriteAllText(path, parallelFileText.Replace("\r\n", "\n"));
        }
    }
}