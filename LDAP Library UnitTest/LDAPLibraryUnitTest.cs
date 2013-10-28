using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LDAPLibrary;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Net;

namespace LDAP_Library_UnitTest
{
	[TestClass]
	public class LDAPLibraryUnitTest
	{
		//Class fields for the test
		ILDAPManager LDAPManagerObj;                 //LDAPLibrary
		string[] LDAPMatchSearchField;				 //Field to search

		[TestMethod]
		public void testCompleteInitLibrary()
		{

			Dictionary<string, string[]> tempAttributes = new Dictionary<string, string[]>()
			{
				//aggiungere inizializzare così il dizionario
				{	"userPassword", new string[]{ConfigurationManager.AppSettings["LDAPAdminUserPassword"]}	}
			};

			LDAPManagerObj = new LDAPManager(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
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

			Assert.AreEqual(LDAPManagerObj.getLDAPMessage(), "LDAP LIBRARY INIT SUCCESS");
		}

		[TestMethod]
		public void testStandardInitLibrary()
		{

			Dictionary<string, string[]> tempAttributes = new Dictionary<string, string[]>()
			{
				//aggiungere inizializzare così il dizionario
				{	"userPassword", new string[]{ConfigurationManager.AppSettings["LDAPAdminUserPassword"]}	}
			};

			LDAPManagerObj = new LDAPManager(ConfigurationManager.AppSettings["LDAPAdminUserDN"],
												ConfigurationManager.AppSettings["LDAPAdminUserCN"],
												ConfigurationManager.AppSettings["LDAPAdminUserSN"],
												tempAttributes,
												ConfigurationManager.AppSettings["LDAPServer"],
												ConfigurationManager.AppSettings["LDAPSearchBaseDN"]
												);

			Assert.AreEqual(LDAPManagerObj.getLDAPMessage(), "LDAP LIBRARY INIT SUCCESS");
		}

		[TestMethod]
		public void testAdminConnect()
		{
			//Init the DLL
			testCompleteInitLibrary();

			//Connect with admin user
			LDAPManagerObj.connect();

			//Assert the behavior of DLL
			Assert.AreEqual(LDAPManagerObj.getLDAPMessage(), "LDAP CONNECTION SUCCESS");

		}

		[TestMethod]
		public void testCreateUser()
		{
			LDAPUser testLDAPUser = setupTestUser();
			//Init the DLL and connect the admin
			testAdminConnect();

			//Create user
			bool result = LDAPManagerObj.createUser(testLDAPUser);

			//Assert the correct operations
			Assert.IsTrue(result);
			Assert.AreEqual(LDAPManagerObj.getLDAPMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Create User Operation Success");

			result = LDAPManagerObj.deleteUser(testLDAPUser);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void testDeleteUser()
		{
			//Set the test user
			LDAPUser testLDAPUser = setupTestUser();

			//Init the DLL and connect the admin
			testAdminConnect();

			//Create LDAPuser to delete.
			bool result = LDAPManagerObj.createUser(testLDAPUser);

			Assert.IsTrue(result);

			//Delete user
			result = LDAPManagerObj.deleteUser(testLDAPUser);

			//Assert the correct operations
			Assert.IsTrue(result);
			Assert.AreEqual(LDAPManagerObj.getLDAPMessage(), "LDAP USER MANIPULATION SUCCESS: " + "Delete User Operation Success");
		}

		[TestMethod]
		public void testSearchUser()
		{
			testAdminConnect();

			string[] userIDToSearch = new string[2]
			{
				"Fabio",
				"Matteo"
			};
			List<string> userAttributeToReturnBySearch = new List<string>()
			{
				"description"
			};

			List<LDAPUser> returnUsers = new List<LDAPUser>();

			bool result = LDAPManagerObj.searchUsers(userAttributeToReturnBySearch, userIDToSearch, out returnUsers);

			Assert.IsTrue(result);
			Assert.AreEqual(returnUsers.Count, userIDToSearch.Length);
			Assert.AreEqual(returnUsers[0].getUserCn(), "Fabio");
			Assert.AreEqual(returnUsers[0].getUserAttribute("description")[0], "descrizione di fabio");
			Assert.AreEqual(returnUsers[1].getUserCn(), "Matteo");
			Assert.AreEqual(returnUsers[1].getUserAttribute("description")[0], "descrizione di matteo");
		}

		[TestMethod]
		public void testModifyUserAttribute()
		{
			testAdminConnect();
			LDAPUser testLDAPUser = setupTestUser();
			LDAPManagerObj.createUser(testLDAPUser);
			List<LDAPUser> returnUsers = new List<LDAPUser>();
			string userAttributeName = "description";
			string userAttributeValue = "description Modified";
				

			bool result = LDAPManagerObj.modifyUserAttribute(DirectoryAttributeOperation.Replace, testLDAPUser, userAttributeName, userAttributeValue);

			Assert.IsTrue(result);

			result = LDAPManagerObj.searchUsers(
				new List<string> { "description" },
				LDAPMatchSearchField,
				out returnUsers);

			Assert.IsTrue(result);
			Assert.AreEqual(returnUsers[0].getUserCn(), testLDAPUser.getUserCn());
			Assert.AreEqual(returnUsers[0].getUserAttribute("description")[0], userAttributeValue);

			result = LDAPManagerObj.deleteUser(testLDAPUser);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void testChangeUserPassword()
		{
			testAdminConnect();
			string newPassword = "pippo";
			LDAPUser testUser = setupTestUser();

			//Create the user
			bool result = LDAPManagerObj.createUser(testUser);

			Assert.IsTrue(result);

			//Perform change of password
			result = LDAPManagerObj.changeUserPassword(testUser, newPassword);
			Assert.IsTrue(result);

			List<string> userAttributeToReturnBySearch = new List<string>()
			{
				"userPassword"
			};
			List<LDAPUser> returnUsers = new List<LDAPUser>();

			result = LDAPManagerObj.searchUsers(userAttributeToReturnBySearch, LDAPMatchSearchField, out returnUsers);

			Assert.IsTrue(result);
			Assert.AreEqual(returnUsers[0].getUserAttribute("userPassword")[0], "pippo");

			result = LDAPManagerObj.deleteUser(testUser);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void testUserConnect()
		{

			//PARTI COMMENTATE PER FARE I TEST SU UDINE DOVE NON SI HANNO I PERMESSI DI SCRITTURA.

			testAdminConnect();
			LDAPUser testUser = setupTestUser();

			//bool result = LDAPManagerObj.createUser(testUser);

			//Assert.IsTrue(result);

			NetworkCredential testUserCredential = new NetworkCredential(
				testUser.getUserDn(),
				testUser.getUserAttribute("userPassword")[0],
				"");

			bool result = LDAPManagerObj.connect(testUserCredential, 
						Convert.ToBoolean(ConfigurationManager.AppSettings["secureSocketLayerFlag"]),
						Convert.ToBoolean(ConfigurationManager.AppSettings["transportSocketLayerFlag"]),
						Convert.ToBoolean(ConfigurationManager.AppSettings["ClientCertificationFlag"]));

			Assert.IsTrue(result);

			//testAdminConnect();
	
			//result = LDAPManagerObj.deleteUser(testUser);

			//Assert.IsTrue(result);
		}

		[TestMethod]
		public void testSearchUserAndConnect() 
		{

			//PARTI COMMENTATE PER FARE I TEST SU UDINE DOVE NON SI HANNO I PERMESSI DI SCRITTURA.

			testAdminConnect();
			LDAPUser testLDAPUser = setupTestUser();

			//bool result = LDAPManagerObj.createUser(testLDAPUser);

			//Assert.IsTrue(result);
			
			bool result = LDAPManagerObj.searchUserAndConnect(LDAPMatchSearchField[0], testLDAPUser.getUserAttribute("userPassword")[0]);

			Assert.IsTrue(result);

			//testAdminConnect();

			//result = LDAPManagerObj.deleteUser(testLDAPUser);

			//Assert.IsTrue(result);
		}

		private LDAPUser setupTestUser()
		{
			string userDN = "cn=uptest,ou=servizio,ou=utenti,dc=uniud,dc=it";
			string userCN = "uptest";
			string userSN = "uptest";
			Dictionary<string, string[]> attribute = new Dictionary<string, string[]>()
			{
				//aggiungere inizializzare così il dizionario
				{	"userPassword", new string[]{"606FSxdklf7q"}	},
				{   "uid", new string[]{"uptest"}	}
			};

			LDAPUser testLDAPUser = new LDAPUser(userDN, userCN, userSN, attribute);

			//
			if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("cn"))
				LDAPMatchSearchField = new string[1] { testLDAPUser.getUserCn() };
			else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("dn"))
				LDAPMatchSearchField = new string[1] { testLDAPUser.getUserDn() };
			else if (ConfigurationManager.AppSettings["LDAPMatchFieldUsername"].Equals("sn"))
				LDAPMatchSearchField = new string[1] { testLDAPUser.getUserSn() };
			else
				LDAPMatchSearchField = new string[1] {
						testLDAPUser.getUserAttribute( ConfigurationManager.AppSettings["LDAPMatchFieldUsername"] )[0]
					};

			//Set the test user
			return testLDAPUser;
		}
	}
}