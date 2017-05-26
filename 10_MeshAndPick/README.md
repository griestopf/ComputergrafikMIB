# Mesh & Pick 

## Lernziele

- Mesh als Geometrie-Bausteine verstehen
  - Aufbau einer Mesh-Komponente
  - Algorithmisch Meshes erzeugen
- Picking: Mit der Maus Szenenbestandteile 
  - Dem Picking bei der Arbeit zusehen
  - Selbst Objekte picken

## Meshes

In der Solution MeshPick.sln wird ein einzelner rotierender Würfel wird angezeigt. 

> **TODO** Zur Wiederholung/Übung/ zum Verständnis:
> 
> - Identifiziert den Teil, der die Würfelanimation (Rotation) implementiert
>   - Was macht die Methode 
>      [`M.MinAngle()`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Math/Core/M.cs#L429)?
>      Warum wird sie aufgerufen? 
> - An welcher Position und welcher Orientierung steht die Kamera? Die Anweisungen
>   ```C#
>   // Setup the camera 
>   RC.View = float4x4.CreateTranslation(0, 0, 40) * float4x4.CreateRotationX(-(float) Atan(15.0 / 40.0));
>   ```
>   beschreiben von links nach rechts und jeweils mit negativ zu interpretierenden Parametern die Transformationen,
>   die auf die Kamera ausgeführt wird, und zwar ausgehend von einer Kamera im Koordinaten-Ursprung `(0, 0, 0)`,
>   die entlang der positiven Z-Achse schaut. Rotationen beziehen sich dabei immer auf den Koordinaten-Ursprung
>   und NICHT etwa auf den Mittelpunt der (ggf. bereits verschobenen) Kamera.
>
>   - Welchem Winkel in Grad entspricht [`Atan(15.0 / 40.0)`](https://msdn.microsoft.com/de-de/library/system.math.atan(v=vs.110).aspx)?
>   - Zeichnet Position und Orientierung der Kamera und die Position des Würfels in einer Seitenansicht 
>     (Y-Z-Achsen) des Weltkoordinatensystems auf.

Der Würfel wird, wie in den vorangegangenen Beispielen auch, als ein Objekt vom Typ 
[`MeshComponent`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Serialization/MeshComponent.cs#L10)
in die Komponentenliste eingehängt. Diese Komponente wird, gleich mit würfelförmiger Geometrie befüllt, von der
Methode `SimpleMeshes.CreateCuboid(new float3(10, 10, 10))` erstellt und zurückgegeben. 
Wir wollen uns ansehen, woraus die Würfel-Geometrie besteht.

> **TODO**
>
> - Schaut Euch die Implementierung von 
>   [`SimpleMeshes.CreateCuboid()`](https://github.com/griestopf/ComputergrafikMIB/blob/master/10_MeshAndPick/Core/SimpleMeshes.cs#L11
)
>    an. _Tipp:_ Ihr könnte mit gedrückter `Strg`-Taste direkt im Visual Studio Editor auf den Methodenaufruf klicken.
> - Seht Euch den Inhalt der Mesh-Komponente im Debuggeran:
>   - In der Methode `Init()`: Setzt einen Breakpoint in die nächste Zeile unter den Aufruf von `_scene = CreateScene();`
>
>     ![Breakpoint](_images/Breakpoint.png)
>
>     Dazu einfach mit der Maus in der grauen Spalte vor der entsprechenden Zeile klicken.
>
>   - Startet den das Programm wie üblich im Debugger über den grünen "Play"-Button (im Desktop Build).
>     *Ergebnis*: Der Programm-Ablauf hält am roten Breakpoint an.
>   - Öffnet das Watch-Fenster des Debuggers (Menü->Debug->Windows->Watch->Watch 1) und fügt als zu beobachtende
>     Variable folgenden Ausdruck ein: `_scene.Children[0].Components[2]`. Es soll also die als **drittes**  
>     (Null-basierter Index; **2**) eingefügte Komponente des **ersten** Kindes (Null-basierter Index; **0**)
>     unserer Szene beobachtet werden. Das ist natürlich die Mesh-Komponente.
>
>     ![Mesh im Watch-Fenster](_images/WatchMesh.png)
>
>   - Diese enthält diverse Arrays, u.A: `Vertices`, `Normals` und `Triangles`. Klappt
>     die Arrays im Watch-Fenster auf und seht Euch die Inhalte an. Vergegenwertigt Euch, dass dies
>     das Resultat des Aufrufs von 
>     [`SimpleMeshes.CreateCuboid()`](https://github.com/griestopf/ComputergrafikMIB/blob/master/10_MeshAndPick/Core/SimpleMeshes.cs#L11
)
>     ist.

### Vertices

Wir wollen nun verstehen, wie diese Daten einen Würfel erzeugen. Zunächst mal betrachten wir den Inhalt 
des `Vertices` Array. Wie uns der Name sagt, sind das die Eckpunkte unserer Geometrie, an denen die Flächen
aufgehängt sind. Wie wir sehen, sind dort 3D-Positionen angegeben und diese liegen alle 5 Einheiten
in jeweils beid möglichen Richtungen entlang jeder Hauptachse (x, Y und Z) vom Ursprung entfent. 

Damit liegen wohl alle Punkte an den Eckpunkten eines Würfels mit dem Zentrum in `(0, 0, 0)` und der Kantenlänge 10
(jeweils von -5 bis 5 - so haben wir es ja im Aufruf von `SimpleMeshes.CreateCuboid(new float3(10, 10, 10))`
angegeben).

> **TODO**
> - Falls das nicht klar ist, zeichnet ein paar der Vertices in ein 3D-Koordinatensystem ein.

Eine Frage stellt sich jedoch: Warum sind es 24 Array-Einträge? Ein Würfel hat doch nur 8 Eckpunkte und dies
ist auch die Anzahl der überhaupt möglichen unterschiedlichen Eckpunkte mit den Koordinaten "Betrag von 5 in
allen Dimensionen". Wie wir an den Array-Einträgen sehen, existiert jeder Eckpunkt dann auch drei mal.

Diese Frage, warum hier offenbar drei mal soviel Eckpunkte angegeben sind wie notwendig, klären wir unten, 
wenn es um Normalen geht. 

### Triangles

FUSEE versteht nur Meshes, die aus Dreicken aufgebaut sind. Sollen Flächen mit mehr Eckpunkten dargestellt
werden, müssen diese aus Dreiecken zusammengepuzzelt werden. Da ein Würfel aus sechs Quadraten besteht, muss
jedes Quadrat aus zwei Dreiecken gebildet werden. Der Array `Triangles` enthält die Information, welche Eckpunkte
mit welchen anderen Eckpunten im `Vertices`-Array Dreiecke bilden. Dazu wird der Inhalt des `Triangles` 
folgendermaßen interpretiert:

- Der Array enthält 36 Einträge, allerdings keine 3D-Koordinaten, sondern Ganzzahl-Werte 
  ([`ushort`](https://docs.microsoft.com/de-de/dotnet/articles/csharp/language-reference/keywords/ushort),
  ähnlich wie int). 
  Wie man sieht liegen diese Arrayeinträge im Bereich [0..23]. Diese Zahlen sind Indizes in den `Vertices`
  Array (und in den `Normals` Array, aber dazu später...).
- Jeweils drei aufeinanderfolgende Indizes im Array bilden ein Dreieck, d.h. die ersten drei Einträge,
  `0`. `6` und `3` bedeuten, dass die an Positionen 0, 6, und 3 im `Vertices`-Array-abgespeicherten Eckpunte
  ein Dreieck bilden. Dann kommen im `Triangles` array die drei Einträge `3`, `6` und `9`. Somit bilden 
  die drei Punkte, die man an diesen Indizes im `Vertices`-Array findet, den nächsten Eintrag.

  ![Triangles Array](_images/Triangles.png)

> **TODO**
> - Zeichnet die ersten vier im `Triangles`-Array angegebenen Dreiecke (d.h. die ersten 12 Einträge verwenden!)
>   in ein 3D-Koordinatensystem ein.

Damit ist klar, dass die 36 Einträge insgesamt 12 Dreiecke (12 * 3 = 36) aufspannen. Das sind genau zwei Dreiecke, 
um jede der sechs quadratischen Würfelflächen darzustellen.

### Normals

Wie bereits im ersten Teil der Veranstaltung klar wurde, wird die Farbgebung der Oberflächen über
Normalenvektoren beeinflusst. Diese geben die Ausrichtung der Fläche im Raum an. Um gerundete Oberflächen
zu simulieren (indem kontinuierliche Farbverläufe wie bei gerundeten Flächen errechnet werden),
werden Normalen nicht pro Fläche oder pro Dreieck angegeben, sondern pro Eckpunkt. Somit enthält der 
`Normals` Array genauso viel Einträge, wie der `Vertices` Array (nämlich 24). Korrespondierende Indizes in 
beiden Array liefern die Koordinate und die Normale eines Eckpunktes. Da ein Würfel nicht aus gerundeten
sondern aus ebenen Flächen besteht, die an deutlich sichtbaren Kanten aufeinander stoßen sollen, muss jeder
Eckpunkt drei mal vorhanden sein, und zwar mit unterschiedlichen Normalen. Nur so können im 
`Triangles`-Array Eckpunkte indiziert werden, die für die jeweilige Flächenausrichtung die passende Normale
besitzen. Folgende Skizze verdeutlicht den Aufbau des Würfels aus Eckpunkten und Normalen und gibt die Indizes 
der Eckpunkte jeweils mit unterschiedlichen Normalen wieder.

![Cube Normalen](_images/VertsAndNormals.png)

> **TODO**
>
> - Sucht beliebige Indizes im 'Triangles'Array, findet jeweils den damit identifzierten Eckpunt im 'Vertices'-Array 
>   und die dazugehörende Normale im `Normals`-Array und vergleicht diese mit der Skizze.










      






