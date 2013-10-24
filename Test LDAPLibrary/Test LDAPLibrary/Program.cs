using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using LDAPLibrary;
using System.Net;
using System.DirectoryServices.Protocols;


namespace Test_LDAPLibrary
{
    class Program
    {

        //Class fields for the test
        static ILDAPManager LDAPManagerObj;                 //LDAPLibrary
        static bool result;                                 //status after the LDAPLibrary call
        static Dictionary<string, string[]> tempAttributes;           //Temp dictionary for the user attribute texts
        static string[] tempStringArray;                    //Temp String Array for populate the dictionary
        static LDAPUser testLDAPUser;                       //Temp LDAPUser used to test creation, deletion, modification.

        static void Main(string[] args)
        {
            Console.WriteLine("Starting test the LDAPLibrary: Read Parameters from config and create the library object");

            Init();

			#region Setup Test User

			//Console.WriteLine("Insert the user test DN,CN,SN divide one by one by breaklines!:");
			//string tempDN = Console.ReadLine();
			//string tempCN = Console.ReadLine();
			//string tempSN = Console.ReadLine();

			//Console.WriteLine("Insert the user test attributes:");
			//tempAttributes.Clear();
			//while (true)
			//{

			//	Console.Write("Insert attribute key(breakpoint to jum over): ");
			//	string tempAttributeKey = Console.ReadLine();
			//	if (string.IsNullOrEmpty(tempAttributeKey)) break;
			//	else
			//	{
			//		Console.Write("How many attribute want insert: ");
			//		int attributeNumber = Int32.Parse(Console.ReadLine());

			//		if (attributeNumber <= 0) continue;

			//		string[] tempAttributeValues = new string[attributeNumber];

			//		for (int i = 0; i < attributeNumber; i++)
			//		{
			//			Console.Write("Insert attribute value: ");
			//			tempAttributeValues[i] = Console.ReadLine();
			//		}


			//		tempAttributes.Add(tempAttributeKey, tempAttributeValues);

			//	}
			//}

			//setupTestUser(tempDN, tempCN, tempSN, tempAttributes);

			#endregion



            //testBocconi();


            testConnect();

            testCreateUser();

            #region Search User

            Console.WriteLine("Test the LDAP Library: step 3 SEARCH USER");

            
            Console.WriteLine("Insert the user list to search ( divide one by one by breaklines - stop all with empty string):");
            List<string> tempUserList = new List<string>();
            while (true)
            {
                string temp = Console.ReadLine();
                if (string.IsNullOrEmpty(temp)) break;
                else tempUserList.Add(temp);

            }

            Console.WriteLine("Insert the attribute list to return in the search ( divide one by one by breaklines - stop all with empty string):");
            List<string> tempUserAttributeList = new List<string>();
            while (true)
            {
                string temp = Console.ReadLine();
                if (string.IsNullOrEmpty(temp)) break;
                else tempUserAttributeList.Add(temp);

            }

            testSearchUser(tempUserList.ToArray(), tempUserAttributeList); 

            #endregion

            #region Modify User Attribute

            Console.WriteLine("Insert the user attribute key to modify: ");
            string tempUserAttributeKey = Console.ReadLine();
            Console.WriteLine("Insert the user attribute key to modify: ");
            string tempUserAttributeValue = Console.ReadLine();

            testModifyUserAttribute(DirectoryAttributeOperation.Replace, tempUserAttributeKey, tempUserAttributeValue);

            #endregion

            #region Modify User Password

            Console.WriteLine("Insert the new user password: ");
            string tempNewUserPassword = Console.ReadLine();
            testModifyUserPassword(tempNewUserPassword); 

            #endregion

            testSearchAndConnect();

            testDeleteUser();

        }

        /// <summary>
        /// Init methods for set up the library
        /// </summary>
        private static void Init() 
        {

            //Set up all the field used in future.
            tempAttributes = new Dictionary<string, string[]>();
            tempStringArray = new string[1];
            tempStringArray[0] = ConfigurationManager.AppSettings["LDAPAdminUserPassword"];
            tempAttributes.Add("userPassword", tempStringArray); //KEEP THE SAME KEY FOR PASSWORD

            LDAPManagerObj = new LDAPManager(	ConfigurationManager.AppSettings["LDAPAdminUserDN"],
												ConfigurationManager.AppSettings["LDAPAdminUserCN"],
												ConfigurationManager.AppSettings["LDAPAdminUserSN"],
												tempAttributes,
												ConfigurationManager.AppSettings["LDAPServer"],
												ConfigurationManager.AppSettings["LDAPSearchBaseDN"],
												ConfigurationManager.AppSettings["LDAPServerDomain"],
												Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
												Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
												Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]),
												ConfigurationManager.AppSettings["clientCertificatePath"],
												Convert.ToBoolean(ConfigurationManager.AppSettings["enableLDAPLibraryLog"]),
												ConfigurationManager.AppSettings["LDAPLibraryLogPath"],
												ConfigurationManager.AppSettings["LDAPUserObjectClass"],
												ConfigurationManager.AppSettings["LDAPMatchFieldUsername"]
												);
		}

        private static void setupTestUser(string userDN, string userCN, string userSN, Dictionary<string, string[]> attribute)
        {
            testLDAPUser = new LDAPUser(userDN, userCN, userSN, attribute);
        }

        /// <summary>
        /// Test the connection of the DLL
        /// REQUIREMENT: RUN ONLY AFTER INIT
        /// </summary>
        private static void testConnect() 
        {

            #region Connect

            Console.WriteLine("Test the LDAP LIbrary: step 1 CONNECT");

            //Call the DLL and execute the operation
            result = LDAPManagerObj.connect();

            if (result == true)
                Console.WriteLine("Connection Success");
            else
            {
                Console.WriteLine("Connection Failed, with this error: ");
                Console.WriteLine(LDAPManagerObj.getLDAPMessage());
            }
            Console.ReadLine();

            #endregion

        }

        /// <summary>
        /// User creation test
        /// REQUIREMENTS: YOU MUST HAVE WRITE RIGHTS!! RUN ONLY AFTER INIT
        /// </summary>
        private static void testCreateUser() 
        {

            #region Create User

            Console.WriteLine("Test the LDAP Library: step 2 CREATE USER");

            //Call the DLL and execute the creation
            result = LDAPManagerObj.createUser(testLDAPUser);

            if (result == true)
                Console.WriteLine("Creation Success");
            else
            {
                Console.WriteLine("Creation Failed, with this error: ");
                Console.WriteLine(LDAPManagerObj.getLDAPMessage());
            }

            Console.ReadLine();

            #endregion

        }

        /// <summary>
        /// Test search user creation 
        /// </summary>
        /// <param name="searchUsers">List of users to search</param>
        /// <param name="otherAttributes">List of the attribute to return in the search</param>
        private static void testSearchUser(string[] searchUsers, List<string> otherAttributes) 
        {

            #region Search User

            if (searchUsers == null) searchUsers = new string[0];
            if (otherAttributes == null) otherAttributes = new List<string>();
            List<LDAPUser> returnUsers = new List<LDAPUser>();

            result = LDAPManagerObj.searchUsers(otherAttributes, searchUsers, out returnUsers);

            if (result == false)
            {
                Console.WriteLine("search Failed, with this error: ");
                Console.WriteLine(LDAPManagerObj.getLDAPMessage());
            }
            else
            {
                Console.WriteLine("Search Result: ");

                foreach (LDAPUser userReturn in returnUsers)
                {
                    Console.WriteLine("DN: " + userReturn.getUserDn());
                    Console.WriteLine("CN: " + userReturn.getUserCn());
                    Console.WriteLine("SN: " + userReturn.getUserSn());
                    foreach (string key in userReturn.getUserAttributeKeys())
                    {
                        Console.WriteLine(key + ": " + userReturn.getUserAttribute(key)[0]);
                    }
                }
            }

            Console.ReadLine();

            #endregion

        }

        /// <summary>
        /// Modify an user attribute.
        /// REQUISITI: MUST HAVE WRITE RIGHTS.
        /// </summary>
        private static void testModifyUserAttribute(DirectoryAttributeOperation Op,string attributekey, string newAttributeValue) 
        {

            #region Modify User Attribute

            Console.WriteLine("Test the LDAP Library: step 4 MODIFY USER ATTRIBUTE");

            result = LDAPManagerObj.modifyUserAttribute(Op, testLDAPUser, attributekey, newAttributeValue);

            if (result == true)
                Console.WriteLine("Modify Success");
            else
            {
                Console.WriteLine("Modify Failed, with this error: ");
                Console.WriteLine(LDAPManagerObj.getLDAPMessage());
            }

            Console.ReadLine();

            #endregion


        }

        /// <summary>
        /// Test the modification of user Password
        /// REQUIREMENTS: NEED WRITE RIGHTS
        /// </summary>
        /// <param name="newPassowrd">new user password</param>
        private static void testModifyUserPassword(string newPassowrd) 
        {

            #region Modify User Password

            Console.WriteLine("Test the LDAP Library: step 5 MODIFY USER PASSWORD");

            result = LDAPManagerObj.changeUserPassword(testLDAPUser, newPassowrd);

            if (result == true)
                Console.WriteLine("Changing Pwd Success");
            else
            {
                Console.WriteLine("Changing Pwd Failed, with this error: ");
                Console.WriteLine(LDAPManagerObj.getLDAPMessage());
            }

            Console.ReadLine();

            #endregion

        }

        /// <summary>
        /// Test the LDAPUser Delete
        /// </summary>
        private static void testDeleteUser() 
        {

            #region Delete User

            Console.WriteLine("Test the LDAP Library: step 6 DELETE USER");

            result = LDAPManagerObj.deleteUser(testLDAPUser);

            if (result == true)
                Console.WriteLine("Deletion Success");
            else
            {
                Console.WriteLine("Deletion Failed, with this error: ");
                Console.WriteLine(LDAPManagerObj.getLDAPMessage());
            }

            Console.ReadLine();

            #endregion

        }

        private static void testSearchAndConnect() 
        {

            #region Search & Connect
            Console.WriteLine("Try to search and connect in one shot!");

            result = LDAPManagerObj.searchUserAndConnect(testLDAPUser.getUserCn(), testLDAPUser.getUserAttribute("userPassword")[0]);

            if (result == true)
                Console.WriteLine("YEAH!");
            else
            {
                Console.WriteLine("operation Failed, with this error: ");
                Console.WriteLine(LDAPManagerObj.getLDAPMessage());
            }
            Console.ReadLine();

            #endregion

        }


        /// <summary>
        /// TempMethod for Bocconi's Test
        /// </summary>
        private static void testBocconi() 
        {
            //Try to connect with test user
            testSearchAndConnect();

            //reconnect with admin for modify the password of test user
            testConnect();
            Console.WriteLine("Insert the new user password: ");
            string tempNewUserPassword = Console.ReadLine();
            testModifyUserPassword(tempNewUserPassword);

            testLDAPUser.setUserAttribute("userPassword", new string[] {tempNewUserPassword} );
            testSearchAndConnect();
        }

    }
}
