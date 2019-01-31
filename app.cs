using System;
using System.Collections.Generic;

namespace IG_Unfollower
{
    public class Application
    {
        // Variables
        protected DriverInsta insta;

        // constructor
        public Application()
        {
            insta = new DriverInsta();
        }

        // destructor
        ~Application()
        {
        }

        public void Run()
        {
            IReadOnlyList<string> usersToKeep = new List<string> {"katie.soloo", "special_soy"};

            insta.Login();
            insta.UnfollowAll(usersToKeep);
        }
    }
}
