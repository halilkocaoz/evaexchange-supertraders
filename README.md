#

## Installation & Run

- `git clone https://github.com/halilkocaoz/evaexchange-supertraders.git`
- `cd evaexchange-supertraders && docker pull  && docker-compose up --build`

Web service deploys with Postgres database as dockerized, there will be seeded data, and no need to configure database because deployment does migration. If you need to check out the database from your local database editor, you can check [appsettings.Development.json](/src/appsettings.Development.json) to get connection string.

[Postman Collection](/EvaExchange.API.postman_collection.json)

### Thanks for taking time to review
