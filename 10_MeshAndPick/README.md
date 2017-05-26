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
>     (Null-basierterIndex; **2**) eingefügte Komponente des **ersten** Kindes (Null-basierterIndex; **0**)
>     unserer Szene beobachtet werden.
>
>     ![Mesh im Watch-Fenster](_images/WatchMesh.png)
>
>   - Eine Mesh-Komponente enthält diverse Arrays, u.A: `Vertices`, `Normals` und `Triangles`. Klappt
>     die Arrays im Watch-Fenster auf und seht Euch die Inhalte an. Vergegenwertigt Euch, dass dies
>     das Resultat des Aufrufs von 
>     [`SimpleMeshes.CreateCuboid()`](https://github.com/griestopf/ComputergrafikMIB/blob/master/10_MeshAndPick/Core/SimpleMeshes.cs#L11
)
>     ist.

Wir wollen nun verstehen, wie diese Daten einen Würfel erzeugen. Zunächst mal betrachten wir die Liste
der Vertices und sehen, dass dort 3D-Positionen angegeben sind 

      






