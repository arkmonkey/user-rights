Sometimes, you would want to implement a rights system on your app (e.g., view a certain page, load a certain widget, upload a document, etc.)

And what is typically done is create database tables: a master list of the rights, and one that links it to users.  And on your code logic, you check whether this user has this or that right to that feature.

This library's aim is to implement that feature in as quickly as 5 minutes.  It takes care of creating the tables, and with a few APIs, lets you implement a user rights feature on any of your .Net apps.

##### Scenario #1: Create the DB tables:
string connectionString = "Data Source=...";
string prefix = "tbl_";
DataSourceManager dsm = new DataSourceManager(connectionString, prefix);
if(!dsm.TablesExist()) {
	dsm.BuildTables();
}


##### Scenario #2: determine whether a user has a given right;
string connectionString = "Data Source=...";
string prefix = "tbl_";
DataSourceManager dsm = new DataSourceManager(connectionString, prefix);
Facade facade = new Facade(dsm);
int userId = GetLoggedInUserId();
Right right = new Right("Documents", "Upload");
bool? hasRight = facade.GetUserRight(userId, right);


##### Scenario #3: you have an admin page where you set the rights of a user.
string connectionString = "Data Source=...";
string prefix = "tbl_";
DataSourceManager dsm = new DataSourceManager(connectionString, prefix);
Facade facade = new Facade(dsm);
int userId = GetLoggedInUserId();
List<UserRight> facade.GetUserRights(userId);


##### Scenario #4: Enable/disable the right of a user 
string connectionString = "Data Source=...";
string prefix = "tbl_";
DataSourceManager dsm = new DataSourceManager(connectionString, prefix);
Facade facade = new Facade(dsm);
int userId = GetLoggedInUserId();
Right right = new Right("Admin", "GeneralAccess");
facade.SetUserRight(userId, right, true);


