from django.db import models

class Group(models.Model):
    created_at = models.DateTimeField(auto_now_add=True)

class Participant(models.Model):
    group = models.ForeignKey(Group, on_delete=models.CASCADE, related_name='participants')
    personid = models.CharField(max_length=50)
    def __str__(self):
        return self.personid

class Location(models.Model):
    participant = models.ForeignKey(Participant, on_delete=models.CASCADE, related_name='locations')
    latitude = models.FloatField()
    longitude = models.FloatField()
    timestamp = models.DateTimeField()

    def __str__(self):
        return f"{self.latitude}, {self.longitude} @ {self.timestamp}"