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
        private readonly static string githubPathURL = "/Assets/com.akihiro.nanokontrol2.sdk";
        private readonly static string devopsPathURL = "/Assets/com.TestPackage";

        [Test]
        public void GitHubHTTPSCloneURLToUPMURL()
        {
            var sourceURL = "https://github.com/akihiro0105/nanoKontrol2_Unity.git";
            var targetURL = "git+https://github.com/akihiro0105/nanoKontrol2_Unity.git?path=/Assets/com.akihiro.nanokontrol2.sdk";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, githubPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void GitHubSSHCloneURLToUPMURL()
        {
            var sourceURL = "git@github.com:akihiro0105/nanoKontrol2_Unity.git";
            var targetURL = "git+ssh://git@github.com/akihiro0105/nanoKontrol2_Unity.git?path=/Assets/com.akihiro.nanokontrol2.sdk";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, githubPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void DevopsHTTPSCloneURLToUPMURL()
        {
            var sourceURL = "https://hololab.visualstudio.com/akihiro_sandbox/_git/UPMTest";
            var targetURL = "git+https://hololab.visualstudio.com/akihiro_sandbox/_git/UPMTest?path=/Assets/com.TestPackage";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, devopsPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void DevopsSSHCloneURLToUPMURL()
        {
            var sourceURL = "hololab@vs-ssh.visualstudio.com:v3/hololab/akihiro_sandbox/UPMTest";
            var targetURL = "git+ssh://hololab@vs-ssh.visualstudio.com/v3/hololab/akihiro_sandbox/UPMTest?path=/Assets/com.TestPackage";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, devopsPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void UPMURLToUPMURL()
        {
            var sourceURL = "git+https://github.com/akihiro0105/nanoKontrol2_Unity?path=/Assets/com.akihiro.nanokontrol2.sdk";
            var targetURL = "git+https://github.com/akihiro0105/nanoKontrol2_Unity?path=/Assets/com.akihiro.nanokontrol2.sdk";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL);
            Assert.AreEqual(upmPath, targetURL);
        }
    }
}