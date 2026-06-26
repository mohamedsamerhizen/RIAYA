# Screenshots

Current captured screenshots are stored in `docs/screenshots/`:

- `01-swagger-overview.png` - Swagger/OpenAPI overview from the local RIAYA API.
- `02-appointments-workflow.png` - Appointment endpoints including create, confirm, cancel, check-in, complete, and no-show workflow routes.
- `03-medical-complex-structure.png` - Clinic rooms, departments, and doctor clinic assignments.
- `04-billing-payments.png` - Invoices, medical services, and payments.

Not captured in this environment:

- `05-tests-passed.png` - No real terminal screenshot facility was available. The test result is documented in `README.md` and the final audit report instead.
- `06-docker-running.png` - Docker was installed, but the Docker daemon was not running, so no real `riaya-api` / `riaya-sqlserver` screenshot was captured.

Publication checklist:

- Screenshots must show RIAYA only.
- Screenshots must not show bearer tokens, refresh tokens, secrets, or production credentials.
- Screenshots must not show real patient data.
- README image links must point only to existing files.
- Docker and test screenshots should be added only after capturing real, current local evidence.
