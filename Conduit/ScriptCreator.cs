using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit
{
    class ScriptCreator
    {
        public String masterBaseFileLoc;
        public String parallelBaseFileLoc;
        public String paramTups;
        public String inputTups;
        private static Dictionary<string, string> convertSbatch = new Dictionary<string, string>()
        {
            { "#runTime", "time" },
            { "#numCPUs", "ntasks-per-node"},
            { "#memPerCPU", "mem-per-cpu"}
        };

        public ScriptCreator(String masterBaseFile, String parallelBaseFile, String paramTups, String inputTups)
        {
            this.masterBaseFileLoc = masterBaseFile;
            this.parallelBaseFileLoc = parallelBaseFile;
            this.paramTups = paramTups;
            this.inputTups = inputTups;
        }

        public string compileMasterScript()
        {
            string baseFileText = System.IO.File.ReadAllText(masterBaseFileLoc);
            string[] fileSplit = baseFileText.Split(new String[] { "*&%@iTag" }, StringSplitOptions.None);
            string[] inputs = inputTups.Split(';');
            string masterFileText = "";
            for (int i = 0; i < fileSplit.Length - 1; i++)
            {
                string[] inputSplit = inputs[i].Split(',');
                masterFileText += fileSplit[i] + inputSplit[0] + '=' + inputSplit[1];
            }
            masterFileText += fileSplit[fileSplit.Length - 1];

            return masterFileText.Replace("\r\n", "\n");
        }

        public string compileParallelScript()
        {
            string baseFileText = System.IO.File.ReadAllText(parallelBaseFileLoc);
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
            return parallelFileText.Replace("\r\n", "\n");
        }
    }
}