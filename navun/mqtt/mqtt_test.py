import paho.mqtt.client as mqtt
from django.conf import settings
import json
from datetime import datetime
from django.utils import timezone

def on_connect(mqtt_client, userdata, flags, rc):
   if rc == 0:
       print('Connected successfully')
       mqtt_client.subscribe('django/mqtt')
   else:
       print('Bad connection. Code:', rc)

def on_message(mqtt_client, userdata, msg):
    print(f'Received message on topic: {msg.topic} with payload: {msg.payload}')

    str_data = msg.payload.decode('utf-8')
    try:
        json_data = json.loads(str_data)
    except json.decoder.JSONDecodeError as e:
        print(f"Error decoding JSON: {e}")
        return
    print(json_data)
    from .models import Location, Group, Participant
    try:
        if(json_data["type"] == "update"):
            try:
                group = Group.objects.get(id=json_data["id"])
            except Group.DoesNotExist:
                group = Group.objects.create()
                topic = "django/mqtt"
                msg = f"id: {group.id}"
            
                rc, mid = mqtt_client.publish(topic, msg, qos=2)

            for person in json_data['group']:
                try:
                    participant = Participant.objects.get(group=group, personid=person['id'])
                except Participant.DoesNotExist:
                    participant = Participant.objects.create(group=group, personid=person['id'])
                
                timestamp = timezone.make_aware(datetime.now(), timezone.get_current_timezone())
                Location.objects.create(participant=participant, latitude=person['latitude'], longitude=person['longitude'], timestamp=timestamp)

            all_groups = Group.objects.all()
            for group in all_groups:
                print(f"Group created at: {group.created_at}")

                participants = group.participants.all()
                for participant in participants:
                    print(f"Participant: {participant}")

                    locations = participant.locations.all()
                    for location in locations:
                        print(f"Location: {location}")
        elif(json_data["type"] == "delete"):
            print(json_data["id"])
    except Exception as e:
        print(e)
        
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.tls_set(ca_certs='./server-ca.crt')
client.username_pw_set(settings.MQTT_USER, settings.MQTT_PASSWORD)
client.connect(
    host=settings.MQTT_SERVER,
    port=settings.MQTT_PORT,
    keepalive=settings.MQTT_KEEPALIVE
)
