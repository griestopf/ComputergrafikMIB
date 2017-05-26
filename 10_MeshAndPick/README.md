# Mesh & Pick 

## Lernziele

- Mesh als Geometrie-Bausteine verstehen
  - Aufbau einer Mesh-Komponente
  - Algorithmisch Meshes erzeugen
- Picking: Mit der Maus Szenenbestandteile 
  - Dem Picking bei der Arbeit zusehen
  - Selbst Objekte picken

## Meshes

In der Solution MeshPick.sln startet wird ein einzelner rotierender Würfel wird angezeigt. 

> **TODO** Zur Wiederholung/Übung/ zum Verständnis:
> 
> - Identifiziert den Teil, der die Würfelanimation (Rotation) implementiert
>   - Was macht die Methode 
>      [M.MinAngle()](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Math/Core/M.cs#L429)?
>      Warum wird sie aufgerufen? 
> - An welcher Position und welcher Orientierung steht die Kamera? Die Anweisungen
>   ```C#
>   // Setup the camera 
>   RC.View = float4x4.CreateTranslation(0, 0, 40) * float4x4.CreateRotationX(-(float) Atan(15.0 / 40.0));
>   ```
>   beschreiben von links nach rechts und jeweils mit negativ zu interpretierenden Parametern die Transformationen,
>   die auf die Kamera ausgeführt wird, die normalerweise im Koordinaten-Ursprung `(0, 0, 0)` steht und entlang
>   der positiven Z-Achse schaut. Rotationen beziehen sich dabei immer auf den Koordinaten-Ursprung und NICHT etwa
>   auf den Mittelpunt der (ggf. bereits verschobenen) Kamera.
>
>   - Welchem Winkel in Grad entspricht [`Atan(15.0 / 40.0)`](https://msdn.microsoft.com/de-de/library/system.math.atan(v=vs.110).aspx)?
>   - Zeichnet Position und Orientierung der Kamera und die Position des Würfels in einer Seitenansicht 
>     (Y-Z-Achsen) des Weltkoordinatensystems auf.
      






