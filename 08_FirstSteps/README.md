# First Steps

## Lernziele

Verständnis einer Echtzeit-3D-Anwendung

### Was heißt Echtzeit?

- Bilder müssen so schnell generiert werden, dass eine flüssige
  Animation möglich ist (aktuell > 30 fps)
- Voraussetzung für Interaktion: Benutzereingaben steuern Parameter der
  Bildberechnung, wie z. B. 
  - Position und Orientierung der Kamera im Raum (First Person)
  - Position, Orientierung und Pose von Charactern (Third Person)

### Grundsätzlicher Aufbau einer Echtzeit-3D-Applikation

```C#
START
  Initialisierung

  WHILE NOT
     Eingabegeräte Abfragen
     Szenenparameter Ändern
     Bild Rendern
  END
END
```

Zu Beginn eines Echtzeit 3D-Programmes weren notwendige Initialisierungen vorgenommen,
wie z.B. Laden von 3D-Modellen, Texturen und anderen _Assets_. Aufbau
eines initialen [Szenengraphen](#der-szenengraph).

Dann begibt sich das Programm in eine "Endlos"-Schleife (die nur durch das
Programmende beendet wird). Jeder Schleifendurchlauf erzeugt ein Bild, daher
muss die Schleifenrumpf schnell genug durchlaufen werden können (Zeit
pro Schleifendurchlauf: < 1/30 sec).

Innerhalb dieses Schleifendurchlaufs wird der Status der Eingabegeräte abgefragt,
auf die die Interaktion reagieren soll. Mögliche Eingabegeräte sind z.B.

- Maus (Position, Status der Tasten)
- Tastaur (Status der Tasten - welche sind gedrückt, welche wieder losgelassen)
- Touchscreen (Position(en) der Touchpoints, Gestenerkennung wie z.B. Pinch)
- Gyroskop, Accelerometer, Kompass (Position und Lage im Raum eines Mobilgerätes)
- Position- und Lagesensor von VR-Brillen

Die Eingaben werden dann in Parameter-Änderungen für das nächste zu rendernde
Bild umgerechnet. Schließlich wird das Bild mit den aktuellen Eingaben gerendert.

Als Autor einer FUSEE-Applikation wird diese Struktur (Initialisierung und anschließende "Endlos"-Schleife) bereits vorgegeben. Den folgenden Code kann
man sich als bereits von FUSEE implementiert vorstellen:

```C#
main()  // FUSEE-Start-Methode
{
   Init();     

   for (;;)
   {
      RenderAFrame();
   }
}
```

Als Programmierer einer FUSEE-Applikation muss man "nur noch" die Methoden 
`Init()` und `RenderAFrame()` mit "Leben" füllen. 

Die Methode `RenderAFrame()` wird also bereits aus einer umgebenden Schleife
auferufen!




## Der Szenengraph