using System;
using NDK.Framework;
using System.Data;
using System.DirectoryServices.AccountManagement;

namespace NDK.DemoPlugin {

	#region DemoPlugin class.
	public class DemoPlugin : PluginBase {

		#region Implement PluginBase abstraction.
		/// <summary>
		/// Gets the unique plugin guid.
		/// When implementing a plugin, this method should return the same unique guid every time. 
		/// </summary>
		/// <returns></returns>
		public override Guid GetGuid() {
			return new Guid("{84FC7623-0B20-40E1-96BF-8B7F0A5BBD94}");
		} // GetGuid

		/// <summary>
		/// Gets the the plugin name.
		/// When implementing a plugin, this method should return a proper display name.
		/// </summary>
		/// <returns></returns>
		public override String GetName() {
			return "NDK Demo Plugin";
		} // GetName

		/// <summary>
		/// Run the plugin.
		/// When implementing a plugin, this method is invoked by the service application or the commandline application.
		/// 
		/// If the method finishes when invoked by the service application, it is reinvoked after a short while as long as the
		/// service application is running.
		/// 
		/// Take care to write good comments in the code, log as much as possible, as correctly as possible (little normal, much debug).
		/// </summary>
		public override void Run() {
			this.Log("***** DEMO PLUGIN *****");
			this.Log("This plugin demonstrates how to implement a plugin for the NDK Framework, and using");
			this.Log("some of the functionality it provides.");

			// Logging.
			this.Log("LOGGING");
			this.Log("This is a normal log line.");
			this.LogDebug("This is a debug log line.");
			this.LogError("This is a error log line.");
			this.LogError(new Exception("This is a exception logging."));

			// Configuration.
			this.Log("CONFIGURATION");
			//this.Log("{0} global properties exist in the configuration.", this.GetKeys());		// not implemented.
			this.Log("{0} plugin properties exist in the configuration.", this.GetConfigKeys().Length);
			foreach (String key in this.GetConfigKeys()) {
				foreach (String value in this.GetConfigValues(key)) {
					this.Log("   {0} = {1}", key, value);
				}
			}

			// Arguments.
			this.Log("ARGUMENTS");
			this.Log("{0} arguments passed on the command line.", this.Arguments.Length);
			foreach (String arg in this.Arguments) {
				this.Log("   {0}", arg);
			}

			// Resources.
			this.Log("RESOURCES");
			this.Log("{0} resources exist in the plugin (calling assembly).", this.GetResourceKeys().Length);
			foreach (String key in this.GetResourceKeys()) {
				this.Log("   {0}", key);
			}

			// Email.
			this.Log("E-MAIL");
			this.Log("Sending e-mail with the 'Email message.txt' as message.");
//			this.SendMail("rpc@norddjurs.dk", "NDK Demo Plugin", this.GetResourceStr("Email message.txt", String.Empty));

			// Database.
			this.Log("DATABASE");
			this.Log("Listing databases:");
			using (IDbConnection db = this.GetSqlConnection("DEMO")) {
				using (IDataReader dbReader = this.ExecuteSql(db, "SELECT name FROM master.dbo.sysdatabases ORDER BY name")) {
					while (dbReader.Read() == true) {
						this.Log("   {0}", dbReader["name"]);
					}
				}
			}

			// Users and groups.
			this.Log("USERS AND GROUPS");
			UserPrincipal user = this.GetUser(Environment.UserName); //this.GetCurrentUser();
			if (user != null) {
				this.Log("         Display Name: {0}", user.DisplayName);
				this.Log("        Email Address: {0}", user.EmailAddress);
				this.Log("   Distinguished Name: {0}", user.DistinguishedName);
				this.Log("     Sam Account Name: {0}", user.SamAccountName);
				this.Log("  User Principal Name: {0}", user.UserPrincipalName);
				this.Log("  Security Identifier: {0}", user.Sid);
				this.Log("                 Guid: {0}", user.Guid);
				foreach (GroupPrincipal group in user.GetGroups()) {
					this.Log("                Group: {0} ({1})", group.Name, group.DisplayName);
				}
			}

			// Exception.
			this.Log("EXCEPTION");
			throw new Exception("This happens when an exception is thrown and not caught!");
		} // Run
		#endregion

	} // DemoPlugin
	#endregion

} // NDK.DemoPlugin


// This is a demo configuration file, used by the "NDK Service.exe" program.
// Modify the configuration and save it as "NDK Service.xml" in the same directory as the following files:
//
//	NDK Framework.dll
//	NDK Demo Plugin.dll
//	NDK Service.exe
//	NDK Service.xml
//
// Execute the demo plugin with this command:
//
//	"NDK Service.exe"  {84FC7623-0B20-40E1-96BF-8B7F0A5BBD94}
//

/*

<NdkFrameworkConfiguration>
  
  <!-- Global configuration -->
  <Section Guid="00000000-0000-0000-0000-000000000000">
	<!-- Logging
		Configure the following logging properties:
	
		LogNormal			true | 1
		LogDebug			true | 1
		LogError			true | 1
		LogConsole			true | 1
		LogFile				true | 1 | <full log filename>
		LogWindows			true | 1
		
		The default log filename is the path and name of the executeable file, but with the ".log" extension.
		When configuring a log filename, the directory must exist.
	-->
    <Property Key="LogNormal">
      <Value>True</Value>
    </Property>
    <Property Key="LogDebug">
      <Value>True</Value>
    </Property>
    <Property Key="LogError">
      <Value>True</Value>
    </Property>
    <Property Key="LogConsole">
      <Value>True</Value>
    </Property>
    <Property Key="LogFile">
      <Value>True</Value>
    </Property>
    <Property Key="LogWindows">
      <Value>False</Value>
    </Property>
	
	<!-- SMTP
		Configure the following SMTP properties:

		SmtpHost			The SMTP hostname or IP
		SmtpPort			The SMTP port number
		SmtpFrom			The default FROM address to use, when not specified
	-->
    <Property Key="SmtpHost">
      <Value>smtp.intern.dk</Value>
    </Property>
    <Property Key="SmtpPort">
      <Value>25</Value>
    </Property>
    <Property Key="SmtpFrom">
      <Value>Demo@intern.dk</Value>
    </Property>
	
	<!-- SQL
		Configure the following database connections.

		SqlEngine			0 = MSSQL (default)
		SqlHost<key>		The SQL database hostname
		SqlDatabase<key>	The database on the hostname
		SqlUserid<key>		The userid
		SqlPassword<key>	The password
		
		The key is the identifier of the individual database connection, used when getting the
		database connection.
		If the userid and password are blank/missing, Windows Authentication (SSPI) is used
		om Microsoft SQL Server.
	-->
    <Property Key="SqlHostDEMO">
      <Value>sql.intern.dk</Value>
    </Property>
    <Property Key="SqlDatabaseDEMO">
      <Value>master</Value>
    </Property>

  </Section>
  
  <!-- NDK Demo Plugin -->
  <Section Guid="84FC7623-0B20-40E1-96BF-8B7F0A5BBD94">
    <Property Key="Test key">
      <Value>value one</Value>
      <Value>value two</Value>
      <Value>value three</Value>
    </Property>
  </Section>
  
</NdkFrameworkConfiguration>
 
*/
