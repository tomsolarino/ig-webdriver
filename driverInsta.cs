using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IG_Unfollower
{
    public class DriverInsta
    {
        // Variables
        protected IWebDriver driver;

        // Xpaths for: Login Screen
        protected const string CRED_USER_NAME = "xxxxxxxxxx";
        protected const string CRED_PASS = "xxxxxxxxxx";
        protected const string INPUT_USERNAME = "//input[contains(@class, '_2hvTZ pexuQ zyHYP')][@name = 'username']";
        protected const string INPUT_PASSWORD = "//input[contains(@class, '_2hvTZ pexuQ zyHYP')][@name = 'password']";
        protected const string BUTTON_LOGIN = "//button[contains(text(), 'Log in')]";

        protected const string BUTTON_ACCOUNT_FOLLOWING = "//a[contains(@class, '-nal3') and contains(@href, 'following')]";
        protected const string ELEMS_FOLLOWED_ACCOUNTS = "//div[@class = 'PZuss']/li";
        protected const string FOLLOWED_ACCOUNTS_USERNAME = ".//div[@class = 'd7ByH']/a";
        protected const string BUTTON_UNFOLLOW_1 = ".//button[text() = 'Following']";
        protected const string BUTTON_UNFOLLOW_2 = "//div[contains(@class, 'pbNvD')]//button[contains(text(), 'Cancel')]";
        protected const string ELEM_FOLLOWING_COUNT = "//span[@class = 'g47SY ' and ../text() = ' following']";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:selenium_test_1.DriverInsta"/> class.
        /// </summary>
        public DriverInsta()
        {
            driver = new ChromeDriver("/usr/local/Caskroom/chromedriver/2.41");
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:selenium_test_1.DriverInsta"/> is reclaimed by garbage collection.
        /// </summary>
        ~DriverInsta()
        {

        }

        /// <summary>
        /// Logs in the user
        /// </summary>
        public void Login()
        {
            driver.Url = "https://www.instagram.com/accounts/login/";

            // Enter username and password
            IWebElement inputUsername = driver.FindElement(By.XPath(INPUT_USERNAME));
            IWebElement inputPassword = driver.FindElement(By.XPath(INPUT_PASSWORD));
            IWebElement buttonLogin = driver.FindElement(By.XPath(BUTTON_LOGIN));
            inputUsername.SendKeys(CRED_USER_NAME);
            inputPassword.SendKeys(CRED_PASS);
            buttonLogin.Click();
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Unfollows all accounts except those passed in
        /// </summary>
        /// <param name="_except">Except.</param>
        public void UnfollowAll(IReadOnlyList<string> _except = null)
        {
            _except = _except ?? new List<string>();

            IList<IWebElement> following = FindAccountsFollowed();

            for (int i = following.Count - 1; i >= 0; i--)
            {
                string userName = following[i].FindElement(By.XPath(FOLLOWED_ACCOUNTS_USERNAME)).Text;
                if (_except.Contains(userName))
                    following.RemoveAt(i);
            }

            UnfollowAccounts(following);
        }

        /// <summary>
        /// Unfollows all accounts passed in. 
        /// NOTE: Needs to be viewing the unfollow list
        /// </summary>
        /// <param name="_accounts">Accounts.</param>
        protected void UnfollowAccounts(IList<IWebElement> _accounts)
        {
            IWebElement buttonUnfollow;
            IWebElement buttonAreYouSure;

            foreach (IWebElement i in _accounts)
            {
                Thread.Sleep(1000);
                buttonUnfollow = i.FindElement(By.XPath(BUTTON_UNFOLLOW_1));
                buttonUnfollow.Click();

                Thread.Sleep(1000);
                buttonAreYouSure = driver.FindElement(By.XPath(BUTTON_UNFOLLOW_2));
                buttonAreYouSure.Click();
            }

            Finish();
        }

        /// <summary>
        /// Finds all accounts being followed by this account.
        /// </summary>
        protected List<IWebElement> FindAccountsFollowed()
        {
            driver.Url = "https://www.instagram.com/" + CRED_USER_NAME + "/";

            // Brings up the list of people the account is following
            IWebElement buttonFollowing = driver.FindElement(By.XPath(BUTTON_ACCOUNT_FOLLOWING));
            buttonFollowing.Click();
            Thread.Sleep(2000);

            // Continues refreshing the list of accounts being followed until all the accounts have loaded
            int totalFollowing = Int32.Parse(driver.FindElement(By.XPath(ELEM_FOLLOWING_COUNT)).Text);
            IList<IWebElement> following = driver.FindElements(By.XPath(ELEMS_FOLLOWED_ACCOUNTS));

            while (totalFollowing != following.Count)
            {
                Actions actions = new Actions(driver);
                actions.MoveToElement(following[following.Count - 1]);
                actions.Perform();
                Thread.Sleep(2000);

                following = driver.FindElements(By.XPath(ELEMS_FOLLOWED_ACCOUNTS));
            }

            return new List<IWebElement>(following);
        }

        /// <summary>
        /// Finish this instance.
        /// </summary>
        protected void Finish()
        {
            driver.Quit();
        }
    }
}
