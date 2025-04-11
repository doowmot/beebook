# 🧑‍🤝‍🧑 AceBook (Facebook Clone)

AceBook is a Facebook-style social media platform built as part of a 5-person Agile team during the Makers Bootcamp. It features user sign-up/login, posts, and a friend request system. The project was designed to simulate the experience of developing a full-stack web app in a professional setting using C#, ASP.NET Core, and PostgreSQL.

## 🔧 Tech Stack

- **Frontend**: Razor Pages (ASP.NET MVC)
- **Backend**: C# with ASP.NET Core
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core (EF Core)
- **Testing**: NUnit
- **Tools**: Git, GitHub, Chromedriver

---

## 🚀 Key Features

- User sign-up and authentication
- Post creation and feed display
- Friend request system (send/accept/reject)
- Test-driven development (95%+ test coverage)
- Fully containerized database with programmatic migrations

---

## 👨‍💻 My Contributions

- Helped implement the **friend request system**, managing user-to-user connections
- Wrote **comprehensive NUnit test suites** to ensure high test coverage
- Collaborated via pair programming and GitHub flow in an Agile environment

---

## 🧠 What I Learned

- How to model complex user relationships with many-to-many tables
- Practical TDD using NUnit and C# for backend logic
- Managing EF Core migrations for schema changes
- Communication and version control in a remote Agile team
- Troubleshooting .NET and PostgreSQL integration issues

---

## 📺 Demo Video

[▶️ Watch Team Demo (2 min)](https://youtu.be/hLgDS5df96U)

> *Note: This video was created and narrated by a teammate during our Makers Bootcamp project presentation. I contributed to the friend request system and backend test coverage, and collaborated throughout using Agile workflows.*

---

## 🛠️ Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/doowmot/beebook.git
cd Acebook
```

### 2. Install Entity Framework CLI

```bash
dotnet tool install --global dotnet-ef
```

### 3. Set Up the Databases in `psql`

```sql
CREATE DATABASE acebook_csharp_development;
CREATE DATABASE acebook_csharp_test;
```

### 4. Run Migrations

```bash
DATABASE_NAME=acebook_csharp_development dotnet ef database update
```

### 5. Start the App

```bash
DATABASE_NAME=acebook_csharp_development dotnet watch run
```

Then open [http://localhost:5287](http://localhost:5287) in your browser.

---

## ✅ Running Tests

```bash
dotnet watch run  # in one terminal
dotnet test       # in a second terminal
```

### You may need:

```bash
brew install chromedriver
```

If blocked by macOS, allow access via:  
**System Preferences > Security & Privacy > General > Allow Anyway**

---

## ⚙️ Migrations Guide

To create a new migration:

```bash
dotnet ef migrations add AddTitleToPosts
dotnet ef database update
```

To rollback:

```bash
dotnet ef database update CreatePostsAndUsers
```

Avoid editing applied migrations — use a new one or rollback and reapply.

---

## 🧭 Future Improvements

- Add notifications for new friend requests
- Implement profile pictures and bios
- Improve UI with modern styling
- Set up GitHub Actions for CI

---

## 📸 Screenshots

*(optional — add a screenshot here if you have one)*

---

## 📜 License

MIT – see `LICENSE` file for details.
