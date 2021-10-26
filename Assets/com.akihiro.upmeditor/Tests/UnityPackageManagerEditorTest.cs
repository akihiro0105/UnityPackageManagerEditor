using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using com.akihiro.upmeditor.editor;

namespace com.akihiro.upmeditor.test
{
    public class UnityPackageManagerEditorTest
    {
        private readonly static string githubPathURL = "/Assets/com.akihiro.upmeditor/";
        private readonly static string devopsPathURL = "/Assets/com.TestPackage";

        [Test]
        public void GitHubのHTTPSのCloneURLからUPMURLに変換()
        {
            var sourceURL = "https://github.com/akihiro0105/UnityPackageManagerEditor.git";
            var targetURL = "git+https://github.com/akihiro0105/UnityPackageManagerEditor.git?path=/Assets/com.akihiro.upmeditor/";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, githubPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void GitHubのSSHのCloneURLからUPMURLに変換()
        {
            var sourceURL = "git@github.com:akihiro0105/UnityPackageManagerEditor.git";
            var targetURL = "git+ssh://git@github.com/akihiro0105/UnityPackageManagerEditor.git?path=/Assets/com.akihiro.upmeditor/";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, githubPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void AzureDevOpsのHTTPSのCloneURLからUPMURLに変換()
        {
            var sourceURL = "https://hololab.visualstudio.com/akihiro_sandbox/_git/UPMTest";
            var targetURL = "git+https://hololab.visualstudio.com/akihiro_sandbox/_git/UPMTest?path=/Assets/com.TestPackage";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, devopsPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void AzureDevOpsのSSHのCloneURLからUPMURLに変換()
        {
            var sourceURL = "hololab@vs-ssh.visualstudio.com:v3/hololab/akihiro_sandbox/UPMTest";
            var targetURL = "git+ssh://hololab@vs-ssh.visualstudio.com/v3/hololab/akihiro_sandbox/UPMTest?path=/Assets/com.TestPackage";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, devopsPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void UPMURLからUPMURLに変換()
        {
            var sourceURL = "git+https://github.com/akihiro0105/UnityPackageManagerEditor.git?path=/Assets/com.akihiro.upmeditor/";
            var targetURL = "git+https://github.com/akihiro0105/UnityPackageManagerEditor.git?path=/Assets/com.akihiro.upmeditor/";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL);
            Assert.AreEqual(upmPath, targetURL);
        }
    }
}
