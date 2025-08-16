db = db.getSiblingDB('admin');

print('Creating application user from hardcoded values...');

db.createUser({
  user: "wyd_app_user",
  pwd: "Test_User_Password",
  roles: [
    { role: "enableSharding", db: "admin" },
    { role: "readWrite", db: "wyd" }
  ]
});

print("Switching to 'wyd' database and creating it.");

db = db.getSiblingDB('wyd');

print('✅ MongoDB initialization complete.');