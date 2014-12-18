using System;
using System.Collections.Generic;
using System.Linq;
using LDAPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDAP_Library_UnitTest
{
    [TestClass]
    public class LdapUserUnitTests
    {
        private const string LdapUserDn = "cn=Manager,dc=maxcrc,dc=com";
        private const string LdapUserCn = "Manager";
        private const string LdapUserSn = "test";

        private static readonly Dictionary<string, List<string>> LdapUserAttributes =
            new Dictionary<string, List<string>>
            {
                {"userPassword", new List<string> {"secret"}},
                {"description", new List<string> {"test description"}},
                {"telephoneNumber", new List<string> {"555-54321"}}
            };

        private readonly LdapUser _testUser = new LdapUser(LdapUserDn,
            LdapUserCn,
            LdapUserSn,
            LdapUserAttributes);

        #region Init Unit test

        [TestMethod, TestCategory("LDAPUser Init")]
        public void LdapUserInstanceNullOtherAttribute()
        {
            var user = new LdapUser(LdapUserDn,
                LdapUserCn,
                LdapUserSn,
                null);

            Assert.IsNotNull(user);
        }

        [TestMethod, TestCategory("LDAPUser Init")]
        public void LdapUserInstance()
        {
            var user = new LdapUser(LdapUserDn,
                LdapUserCn,
                LdapUserSn,
                LdapUserAttributes);

            Assert.IsNotNull(user);
        }

        [TestMethod, TestCategory("LDAPUser Init")]
        [ExpectedException(typeof (ArgumentException),
            "The creation of the user with dn null don't throw an exception")]
        public void NullDn()
        {
            new LdapUser(null,
                LdapUserCn,
                LdapUserSn,
                null);
        }

        [TestMethod, TestCategory("LDAPUser Init")]
        [ExpectedException(typeof (ArgumentException),
            "The creation of the user with cn null don't throw an exception")]
        public void NullCn()
        {
            new LdapUser(LdapUserDn,
                null,
                LdapUserSn,
                null);
        }

        [TestMethod, TestCategory("LDAPUser Init")]
        [ExpectedException(typeof (ArgumentException),
            "The creation of the user with sn null don't throw an exception")]
        public void NullSn()
        {
            new LdapUser(LdapUserDn,
                LdapUserCn,
                null,
                null);
        }

        #endregion

        #region Getter Tests

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserAttribute()
        {
            Assert.AreEqual(_testUser.GetUserAttribute("userPassword")[0], "secret");
            Assert.IsInstanceOfType(_testUser.GetUserAttribute("userPassword"), typeof (List<string>));

            Assert.AreEqual(_testUser.GetUserAttribute("description")[0], "test description");
            Assert.IsInstanceOfType(_testUser.GetUserAttribute("description"), typeof (List<string>));

            Assert.AreEqual(_testUser.GetUserAttribute("telephoneNumber")[0], "555-54321");
            Assert.IsInstanceOfType(_testUser.GetUserAttribute("telephoneNumber"), typeof (List<string>));
        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        [ExpectedException(typeof (ArgumentException),
            "if the key requested is not in che dictionary an exception is throw")]
        public void GetUserAttributeNoKey()
        {
            _testUser.GetUserAttribute("NoKey");
        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserAttributes()
        {
            CollectionAssert.AreEqual(_testUser.GetUserAttributes(), LdapUserAttributes);
        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserAttributesKeys()
        {
            CollectionAssert.AreEqual(_testUser.GetUserAttributeKeys(), LdapUserAttributes.Keys.ToArray());
        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserCn()
        {
            Assert.AreEqual(_testUser.GetUserCn(), LdapUserCn);
        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserDn()
        {
            Assert.AreEqual(_testUser.GetUserDn(), LdapUserDn);
        }

        [TestMethod, TestCategory("LDAPUser Getter")]
        public void GetUserSn()
        {
            Assert.AreEqual(_testUser.GetUserSn(), LdapUserSn);
        }

        #endregion

        #region Setter Tests

        [TestMethod, TestCategory("LDAPUser Operations")]
        public void OverwriteUserAttributeList()
        {
            var testUserCopy = new LdapUser(LdapUserDn,
                LdapUserCn,
                LdapUserSn,
                LdapUserAttributes);
            var descriptions = new List<string> {"new test description 1", "new test description 2"};
            var telephoneNumbers = new List<string> {"123456789", "987654321"};

            testUserCopy.OverwriteUserAttribute("description", descriptions);
            testUserCopy.OverwriteUserAttribute("telephoneNumber", telephoneNumbers);

            CollectionAssert.AreEqual(testUserCopy.GetUserAttribute("description"), descriptions);
            CollectionAssert.AreEqual(testUserCopy.GetUserAttribute("telephoneNumber"), telephoneNumbers);
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        public void OverwriteUserAttributeSingle()
        {
            var testUserCopy = new LdapUser(LdapUserDn,
                LdapUserCn,
                LdapUserSn,
                LdapUserAttributes);
            const string descriptions = "new test description 1";
            const string telephoneNumbers = "123456789";

            testUserCopy.OverwriteUserAttribute("description", descriptions);
            testUserCopy.OverwriteUserAttribute("telephoneNumber", telephoneNumbers);

            CollectionAssert.AreEqual(testUserCopy.GetUserAttribute("description"), new List<string> {descriptions});
            CollectionAssert.AreEqual(testUserCopy.GetUserAttribute("telephoneNumber"),
                new List<string> {telephoneNumbers});
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        [ExpectedException(typeof (ArgumentException),
            "if the key requested is not in che dictionary an exception is throw")]
        public void OverwriteUserAttributeListNotExist()
        {
            _testUser.OverwriteUserAttribute("NotExistAttribute", new List<string> {"test"});
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        [ExpectedException(typeof (ArgumentException),
            "if the key requested is not in che dictionary an exception is throw")]
        public void OverwriteUserAttributeSingleNotExist()
        {
            _testUser.OverwriteUserAttribute("NotExistAttribute", "test");
        }


        [TestMethod, TestCategory("LDAPUser Operations")]
        public void CreateUserAttributeList()
        {
            var user = new LdapUser(LdapUserDn,
                LdapUserCn,
                LdapUserSn,
                LdapUserAttributes);
            var newAttribute = new List<string> {"new test value 1", "new test value 2"};

            user.CreateUserAttribute("newAttribute", newAttribute);

            CollectionAssert.AreEqual(user.GetUserAttribute("newAttribute"), newAttribute);
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        [ExpectedException(typeof (ArgumentException),
            "if the key requested is in che dictionary an exception is throw")]
        public void CreateUserAttributeListExist()
        {
            _testUser.CreateUserAttribute("description", new List<string> {"test"});
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        public void CreateUserAttributeSingle()
        {
            var user = new LdapUser(LdapUserDn,
                LdapUserCn,
                LdapUserSn,
                LdapUserAttributes);
            const string newAttribute = "new test value 1";

            user.CreateUserAttribute("newAttribute1", newAttribute);

            CollectionAssert.AreEqual(user.GetUserAttribute("newAttribute1"), new List<string> {newAttribute});
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        [ExpectedException(typeof (ArgumentException),
            "if the key requested is in che dictionary an exception is throw")]
        public void CreateUserAttributeSingleExist()
        {
            _testUser.CreateUserAttribute("description", "test");
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        public void InsertUserAttribute()
        {
            var user = new LdapUser(LdapUserDn,
                LdapUserCn,
                LdapUserSn,
                new Dictionary<string, List<string>>
                {
                    {"description", new List<string> {"test description"}}
                });
            const string addictionalDescription = "new test description Inserted";

            user.InsertUserAttribute("description", addictionalDescription);

            Assert.IsTrue(user.GetUserAttribute("description").Contains(addictionalDescription));

            user.InsertUserAttribute("description", null);
            user.InsertUserAttribute("description", "");

            Assert.IsTrue(user.GetUserAttribute("description").Count == 2);
        }


        [TestMethod, TestCategory("LDAPUser Operations")]
        [ExpectedException(typeof (ArgumentException),
            "if the key requested is not in che dictionary an exception is throw")]
        public void InsertUserAttributeNotExist()
        {
            _testUser.InsertUserAttribute("NotExistAttribute", "test");
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        public void DeleteUserAttribute()
        {
            var user = new LdapUser(LdapUserDn,
                LdapUserCn,
                LdapUserSn,
                new Dictionary<string, List<string>>
                {
                    {"description", new List<string> {"test description"}},
                });

            const string descriptionToDelete = "test description";

            user.DeleteUserAttribute("description", descriptionToDelete);

            Console.WriteLine(user.GetUserAttribute("description"));
            Assert.IsTrue(!user.GetUserAttribute("description").Contains(descriptionToDelete));
            Assert.IsTrue(user.GetUserAttribute("description").Count == 0);
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        [ExpectedException(typeof (ArgumentException),
            "if the key requested is not in che dictionary an exception is throw")]
        public void DeleteUserAttributeNotExist()
        {
            _testUser.DeleteUserAttribute("NotExistAttribute", "test");
        }

        [TestMethod, TestCategory("LDAPUser Operations")]
        [ExpectedException(typeof (ArgumentException),
            "if the value requested is not in che dictionary an exception is throw")]
        public void DeleteUserAttributeValueNotExist()
        {
            _testUser.DeleteUserAttribute("description", "test");
        }

        #endregion
    }
}