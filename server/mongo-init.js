db = db.getSiblingDB('wyd');

db.createUser(
  {
    user: "wyd_app_user",
    pwd: "Use_Current_Universe1_Buffalo",
    roles: [
      { role: "readWrite", db: "wyd" }
    ]
  }
);

print("Application user 'wyd_app_user' created successfully in 'wyd' database.");