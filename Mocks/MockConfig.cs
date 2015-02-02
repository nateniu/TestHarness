using System.Collections.Generic;
using System.ComponentModel.Composition;
using NSubstitute;
using NSubstitute.Core;
using Pearl.Interfaces;
using Pearl.Plugins.Utilities;

namespace Mocks
{
    /// <summary>
    /// The mock config.
    /// </summary>
    public class MockConfig
    {
        /// <summary>
        /// The remote.
        /// </summary>
        public static readonly string Remote = "remote";

        /// <summary>
        /// The entitle.
        /// </summary>
        public static readonly string Entitle = "entitle";

        /// <summary>
        /// The url.
        /// </summary>
        public static readonly string Url = "url";

        /// <summary>
        /// The login.
        /// </summary>
        public static readonly string Login = "login";

        /// <summary>
        /// The details.
        /// </summary>
        public static readonly string Details = "details";

        /// <summary>
        /// The user name.
        /// </summary>
        public static readonly string UserName = "username";

        /// <summary>
        /// The password.
        /// </summary>
        public static readonly string Password = "password";

        /// <summary>
        /// The installid.
        /// </summary>
        public static readonly string Installid = "installid";

        /// <summary>
        /// The industry.
        /// </summary>
        public static readonly string Industry = "industry";

        /// <summary>
        /// The config.
        /// </summary>
        [Export(typeof (IConfigManager))] public IConfigManager Config = Substitute.For<IConfigManager>();

        /// <summary>
        /// The dict.
        /// </summary>
        [Export(typeof (IDictionary<string, string>))] public IDictionary<string, string> dict =
            new Dictionary<string, string>();


        /// <summary>
        /// Initializes a new instance of the <see cref="MockConfig"/> class.
        /// </summary>
        public MockConfig()
        {
            Config.Get(Arg.Any<string>(), Arg.Any<string>()).Returns(FunGet);

            Config.When(x => x.Set(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()))
                .Do(ActSet);


            Config.Set(Remote, Url, "http://dflink.sandbox.demandforced3.com");
            Config.Set(Remote, Login, "/servlet/dflogin");
            Config.Set(Remote, Details, "/servlet/details");
            Config.Set(Entitle, UserName, "optometry_test@demandforce.com");
            Config.Set(Entitle, Password, AESEncryption.EncryptString("optondemand!1"));
            Config.Set(Entitle, Installid, "61D13359-C3A6-301C-8B17-9283A2188A9A");
            Config.Set(Entitle, Industry, "optometry");
        }

        /// <summary>
        /// The fun get.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string FunGet(CallInfo info)
        {
            object[] os = info.Args();
            var s1 = (string) os[0];
            var s2 = (string) os[1];
            string s = s1 + "." + s2;

            if (dict.ContainsKey(s))
            {
                return dict[s];
            }

            return string.Empty;
        }

        /// <summary>
        /// The act set.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        private void ActSet(CallInfo info)
        {
            object[] os = info.Args();
            var s1 = (string) os[0];
            var s2 = (string) os[1];
            var s3 = (string) os[2];
            string s = s1 + "." + s2;

            dict[s] = s3;
        }
    }
}