import json
from django.http import JsonResponse
from mqtt.mqtt_test import client as mqtt_client


def publish_message(request):
    request_data = json.loads("""{
    "topic": "django/mqtt",
    "msg": "Hello from Django"
}""")
    rc, mid = mqtt_client.publish(request_data['topic'], request_data['msg'])
    return JsonResponse({'code': rc})