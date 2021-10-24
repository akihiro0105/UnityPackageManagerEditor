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
        public void GitHub��HTTPS��CloneURL����UPM�p�X���擾�ł���()
        {
            var sourceURL = "https://github.com/akihiro0105/nanoKontrol2_Unity.git";
            var targetURL = "git+https://github.com/akihiro0105/nanoKontrol2_Unity.git?path=/Assets/com.akihiro.nanokontrol2.sdk";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, githubPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void GitHub��SSH��CloneURL����UPM�p�X���擾�ł���()
        {
            var sourceURL = "git@github.com:akihiro0105/nanoKontrol2_Unity.git";
            var targetURL = "git+ssh://git@github.com/akihiro0105/nanoKontrol2_Unity.git?path=/Assets/com.akihiro.nanokontrol2.sdk";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, githubPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void Devops��HTTPS��CloneURL����UPM�p�X���擾�ł���()
        {
            var sourceURL = "https://hololab.visualstudio.com/akihiro_sandbox/_git/UPMTest";
            var targetURL = "git+https://hololab.visualstudio.com/akihiro_sandbox/_git/UPMTest?path=/Assets/com.TestPackage";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, devopsPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void Devops��SSH��CloneURL����UPM�p�X���擾�ł���()
        {
            var sourceURL = "hololab@vs-ssh.visualstudio.com:v3/hololab/akihiro_sandbox/UPMTest";
            var targetURL = "git+ssh://hololab@vs-ssh.visualstudio.com/v3/hololab/akihiro_sandbox/UPMTest?path=/Assets/com.TestPackage";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL, devopsPathURL);
            Assert.AreEqual(upmPath, targetURL);
        }

        [Test]
        public void UPM�p�X����UPM�p�X���擾�ł���()
        {
            var sourceURL = "git+https://github.com/akihiro0105/nanoKontrol2_Unity?path=/Assets/com.akihiro.nanokontrol2.sdk";
            var targetURL = "git+https://github.com/akihiro0105/nanoKontrol2_Unity?path=/Assets/com.akihiro.nanokontrol2.sdk";

            var upmPath = UPMPathConverter.ToUPMPath(sourceURL);
            Assert.AreEqual(upmPath, targetURL);
        }
    }
}
