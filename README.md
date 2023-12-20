# OnlineStore

## Description
The OnlineStore project is a web API built using ASP.NET Core, following the CQRS (Command Query Responsibility Segregation) pattern. It features controllers for managing products, transactions, and users, and utilizes MediatR for mediating between components. The application includes authentication and authorization using JWT tokens, validation of requests with FluentValidation (via MediatR pipeline), and uses Entity Framework Core for database interactions.

### Important notes
This project was developed for a recruitment challenge. While efforts have been made to address various scenarios, there are opportunities for further enhancements.

### External packages used
* MediatR
* Fluent Validation
* Fluent Assertions
* Entity Framework Core

### How to run
No specific requirements are necessary to run the project, as it operates on an in-memory database that is being seeded each time on the startup.
There are 3 users created within `OnlineStore.Infrastructure.Services.DatabaseSeederService`. Their default credentials are: `customer@mail.com / customer123`, `employee@mail.com / employee123`, `admin@mail.com / admin123`.

# Users Controller Endpoints

## `GET /api/user/{id}`

- **Description:** Retrieves user information by ID.
- **Parameters:**
  - `id` (Path): The unique identifier for the user.
- **Authorization:** Requires role `Admin`.
- **Response:** `Ok (200)` with the user information if successful, `NotFound (404)` if the user with `id` is not found.

## `POST /api/user`

- **Description:** Logs in a user.
- **Request Body:** `LoginRequest` - User credentials for login.
- **Example Request Body**
  ```json
  {
    "email": "customer123@mail.com",
    "password": "customer123"
  }
  ```
- **Response:** `Ok (200)` with the login response if successful, `Unauthorized (401)` otherwise. The response includes a flag indicating success and additional information.

### Example Request Body:

```json
{
  "success": true,
  "userId": "1111111-111111-11111-111111111"
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
}


# Product Controller Endpoints
## `GET /api/product`

- **Description:** Retrieves all products.
- **Parameters:**
  - `searchPhrase` (query): Filters products by name.
- **Authorization:** None (Public endpoint).

## `GET /api/product/{id}`

- **Description:** Retrieves a specific product by ID.
- **Parameters:**
  - `id` (path): The unique identifier for the product.
- **Authorization:** None (Public endpoint).

## `POST /api/product`

- **Description:** Adds a new product.
- **Request Body:** `AddProductData` - Data for the new product.
- **Authorization:** Requires roles "Employee" or "Admin."

## `PUT /api/product/{id}`

- **Description:** Updates an existing product.
- **Parameters:**
  - `id` (Path): The unique identifier for the product.
- **Request Body:** `UpdateProductData` - Data for updating the product.
- **Authorization:** Requires roles `Employee` or `Admin`.
- **Response:** `NoContent (204)` if successful, `NotFound (404)` if the product with `id` is not found.

## `DELETE /api/product/{id}`

- **Description:** Deletes a product.
- **Parameters:**
  - `id` (Path): The unique identifier for the product.
- **Authorization:** Requires roles "Employee" or "Admin."
- **Response:** `NoContent (204)` if successful, `NotFound (404)` if the product with `id` is not found.

  
# Transactions Controller Endpoints

## `GET /api/transaction`

- **Description:** Retrieves all transactions.
- **Authorization:** Requires roles `Employee` or `Admin`.

## `GET /api/transaction/{id}`

- **Description:** Retrieves a specific transaction by ID.
- **Parameters:**
  - `id` (path): The unique identifier for the transaction.
- **Authorization:** Requires authentication. The user must be the owner or have roles `Admin` or `Employee`.

## `POST /api/transaction`

- **Description:** Adds a new transaction.
- **Request Body:** `AddTransactionData` - Data for the new transaction.
- **Authorization:** Requires authentication.
- **Response:** `Created (201)` with the `id` of the created transaction if successful, `BadRequest (400)` otherwise.

## `PATCH /api/transaction/{id}`

- **Description:** Updates the status of a transaction.
- **Parameters:**
  - `id` (path): The unique identifier for the transaction.
  - `isAccepted` (query): Boolean indicating whether the transaction is accepted.
- **Authorization:** Requires roles `Employee` or `Admin`.
- **Response:** `NoContent (204)` if successful, `NotFound (404)` if the transaction with `id` is not found.

## `PATCH /api/transaction/{id}/revise`

- **Description:** Revises a transaction by updating the offer.
- **Parameters:**
  - `id` (path): The unique identifier for the transaction.
- **Request Body:** Decimal indicating the revised offer.
- **Authorization:** Requires authentication. The user must be the owner.
- **Response:** `NotFound (404)` if the transaction with `id` is not found or if the user is not the owner. `Ok (200)` with the updated transaction if successful.

# License

This project is licensed under the [MIT License](LICENSE).
