# MQTT - SERVER

## Requirements

Before installing, ensure your system meets the following requirements:

- **PostgreSQL 16**
- **Python 3.10**

## Installation

Follow these steps to get your development environment set up:

1. **Clone the repository**

   First, clone the project repository to your local machine.

   ```bash
   gh repo clone Firotss/Navun
   cd navun
   ```

2. **Install Dependencies**

   Run the following command in your terminal to install the necessary dependencies:

   ```bash
   pip install -r requirements.txt
   ```

3. **Update `settings.py`**

   Open the `settings.py` file in your favorite editor and update it with your MQTT server details:

   ```python
   MQTT_SERVER = "your_mqtt_server"
   MQTT_USER = "your_mqtt_user"
   MQTT_PASSWORD = "your_mqtt_password"
   ```

   For ssl connection change server-ca.crt certificate to actual

## Running the Application

After completing the installation steps, you can run your application using:

```bash
python manage.py runserver
```
