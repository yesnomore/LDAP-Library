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
        static void Main(string[] args)
        {
            Console.WriteLine("Starting test the LDAPLibrary: Read Parameters from config and create the library object");

            //Get the user Login Password and create a temp dictionary for the library
            Dictionary<string, string[]> temp = new Dictionary<string, string[]>();
            string [] tempStringArray = new string[1];
            tempStringArray[0] = ConfigurationManager.AppSettings["LDAPAdminUserPassword"];

            //Active Directory OU=Test LDAP,OU=Apex,dc=apex-net,dc=it
            //OpenLDAP o=ApexNet,ou=People,dc=maxcrc,dc=com
            LDAPUser user2 = new LDAPUser("cn=Enrico,OU=Test LDAP,OU=Apex,dc=apex-net,dc=it", "Enrico", "Benini", null);

            temp.Add("userPassword", tempStringArray); //Importante mantenere la stessa chiave perchè è così sia per OpenLDAP che per AD. La libreria cerca quella.

            ILDAPManager LDAPManager = new LDAPManager(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
                                                       ConfigurationManager.AppSettings["LDAPAdminUserCN"],
                                                       ConfigurationManager.AppSettings["LDAPAdminUserSN"],
                                                       temp,
                                                       ConfigurationManager.AppSettings["LDAPServer"],
                                                       ConfigurationManager.AppSettings["LDAPServerDomain"],
                                                       ConfigurationManager.AppSettings["LDAPBaseDN"],
                                                       Convert.ToBoolean(ConfigurationManager.AppSettings["enableLDAPLibraryLog"]),
                                                       ConfigurationManager.AppSettings["LDAPLibraryLogPath"],
                                                       ConfigurationManager.AppSettings["LDAPUserObjectClass"]);


            Console.WriteLine("Test the LDAP LIbrary: step 1 CONNECT");
            bool result = LDAPManager.connect();

            if (result == true)
                Console.WriteLine("Connection Success");
            else
            {
                Console.WriteLine("Connection Failed, with this error: ");
                Console.WriteLine(LDAPManager.getLDAPMessage());
            }
            Console.ReadLine();

            //Console.WriteLine("Test the LDAP Library: step 2 CREATE USER");

            temp.Clear();
            tempStringArray[0] = "The best apexnet developer";
            temp.Add("description", tempStringArray);
            string[] tempStringArray2 = new string[1];
            tempStringArray2[0] = "ciao";
            temp.Add("userPassword", tempStringArray2);
            LDAPUser user = new LDAPUser("cn=Alessandro,OU=Test LDAP,OU=Apex,dc=apex-net,dc=it", "Alessandro", "Zoffoli", temp);

            //PARTE DISABILITATA MOMENTANEAMENTE PERCHE' NEL SERVER LDAP PUBBLICO CON CUI MI CONNETTO NON HO I PERMESSI DI SCRITTURA

            //result = LDAPManager.createUser(user);

            //if (result == true)
            //    Console.WriteLine("Creation Success");
            //else
            //{
            //    Console.WriteLine("Creation Failed, with this error: ");
            //    Console.WriteLine(LDAPManager.getLDAPMessage());
            //}

            //Console.ReadLine();

            Console.WriteLine("Test the LDAP Library: step 3 SEARCH USER");

            string [] searchUsers = new string[3];
            searchUsers[0] = "stuart";
            searchUsers[1] = "john";
            searchUsers[2] = "carol";

            List<LDAPUser> returnUsers = new List<LDAPUser>();
            List<string> otherAttributes = new List<string>();
            otherAttributes.Add("mail");
            result = LDAPManager.searchUsers("OU=users,DC=testathon,DC=net", otherAttributes, searchUsers, out returnUsers);

            if (result == false)
            {
                Console.WriteLine("search Failed, with this error: ");
                Console.WriteLine(LDAPManager.getLDAPMessage());
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

            Console.WriteLine("Test the LDAP Library: step 4 MODIFY USER ATTRIBUTE");

            //PARTE DISABILITATA MOMENTANEAMENTE PERCHE' NEL SERVER LDAP PUBBLICO CON CUI MI CONNETTO NON HO I PERMESSI DI SCRITTURA

            //result = LDAPManager.modifyUserAttribute(DirectoryAttributeOperation.Replace, user, "description", "modified description");

            //if (result == true)
            //    Console.WriteLine("Modify Success");
            //else
            //{
            //    Console.WriteLine("Modify Failed, with this error: ");
            //    Console.WriteLine(LDAPManager.getLDAPMessage());
            //}

            //Console.ReadLine();

            Console.WriteLine("Test the LDAP Library: step 5 MODIFY USER PASSWORD");

            //PARTE DISABILITATA MOMENTANEAMENTE PERCHE' NEL SERVER LDAP PUBBLICO CON CUI MI CONNETTO NON HO I PERMESSI DI SCRITTURA

            //result = LDAPManager.changeUserPassword(user2, "new password");

            //if (result == true)
            //    Console.WriteLine("Changing Pwd Success");
            //else
            //{
            //    Console.WriteLine("Changing Pwd Failed, with this error: ");
            //    Console.WriteLine(LDAPManager.getLDAPMessage());
            //}

            Console.ReadLine();

            Console.WriteLine("Test the LDAP Library: step 6 DELETE USER");

            //PARTE DISABILITATA MOMENTANEAMENTE PERCHE' NEL SERVER LDAP PUBBLICO CON CUI MI CONNETTO NON HO I PERMESSI DI SCRITTURA

            //result = LDAPManager.deleteUser(user);

            //if (result == true)
            //    Console.WriteLine("Deletion Success");
            //else
            //{
            //    Console.WriteLine("Deletion Failed, with this error: ");
            //    Console.WriteLine(LDAPManager.getLDAPMessage());
            //}

            Console.ReadLine();
            
            Console.WriteLine("Try to search and connect in one shot!");

            bool searchAndConnect = LDAPManager.searchUserAndConnect("OU=users,DC=testathon,DC=net", "john", "john");

            if (searchAndConnect == true)
                Console.WriteLine("YEAH!");
            else
            {
                Console.WriteLine("operation Failed, with this error: ");
                Console.WriteLine(LDAPManager.getLDAPMessage());
            }

            Console.ReadLine();
        }
    }
}
