[README.md](https://github.com/user-attachments/files/28877588/README.md)
# EndTown API

A Social Network REST API built with ASP.NET Core 8, inspired by platforms like Facebook.

## Features

- **Authentication** — JWT Register & Login with BCrypt password hashing
- **Posts** — Create, Read, Delete posts with Image support
- **Likes** — Like & Unlike posts
- **Comments** — Add & Read comments on posts
- **Friendship** — Send, Accept, Reject friend requests
- **Pages** — Create pages, Follow & Unfollow
- **Groups** — Create groups, Join & Leave (Public/Private)
- **User Search** — Search users by username or email
- **Platforms** — Full CRUD for social platforms

## Technologies

- ASP.NET Core 8
- Entity Framework Core
- MS SQL Server
- JWT Bearer Authentication
- BCrypt Password Hashing
- Swagger UI
- Service Layer Architecture
- Repository Pattern
- DTO Pattern

## Architecture

```
Controller  →  Service  →  DbContext  →  Database
HTTP Layer     Business    Data Access    SQL Server
               Logic
```

## Project Structure

```
EndTown/
├── Controllers/        — HTTP endpoints
├── Services/           — Business logic + Interfaces
├── Models/
│   └── Entities/       — DB models + DTOs
├── Data/               — DbContext
├── Migrations/         — DB migrations
└── Program.cs          — DI + Middleware pipeline
```

## API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Auth/register | Register new user |
| POST | /api/Auth/login | Login + get JWT token |

### Posts
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/Posts | Get all posts (Feed) |
| POST | /api/Posts | Create post (Auth) |
| GET | /api/Posts/{id} | Get post by ID |
| DELETE | /api/Posts/{id} | Delete post (Auth) |
| POST | /api/Posts/{id}/like | Like post (Auth) |
| DELETE | /api/Posts/{id}/like | Unlike post (Auth) |
| GET | /api/Posts/{id}/comments | Get comments |
| POST | /api/Posts/{id}/comments | Add comment (Auth) |

### Friendships
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Friendships/send/{id} | Send friend request |
| PUT | /api/Friendships/accept/{id} | Accept request |
| PUT | /api/Friendships/reject/{id} | Reject request |
| GET | /api/Friendships/friends | Get friends list |
| GET | /api/Friendships/pending | Get pending requests |

### Pages
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/Pages | Get all pages |
| POST | /api/Pages | Create page (Auth) |
| POST | /api/Pages/{id}/follow | Follow page |
| DELETE | /api/Pages/{id}/follow | Unfollow page |

### Groups
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/Groups | Get all groups |
| POST | /api/Groups | Create group (Auth) |
| POST | /api/Groups/{id}/join | Join group |
| DELETE | /api/Groups/{id}/leave | Leave group |

### Users
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/Users/search?query= | Search users |
| GET | /api/Users/{id} | Get user profile |

## Getting Started

```bash
# Clone the repository
git clone https://github.com/nodo1121-ende/EndTown-.git

# Update connection string in appsettings.json
# "DefaultConnection": "Server=...;Database=EndTownDb;"

# Run migrations
dotnet ef database update

# Run the project
dotnet run
```

Open Swagger UI: `https://localhost:7160/swagger`

## Author

**Nodar Endeladze** — Junior Backend Developer

- LinkedIn: [linkedin.com/in/nodar-endeladze](https://www.linkedin.com/in/nodar-endeladze-5626b73a3)
- GitHub: [github.com/nodo1121-ende](https://github.com/nodo1121-ende)
