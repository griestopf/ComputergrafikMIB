# First Steps

## Lernziele

- FUSEE installieren und bauen
- Was heißt Echtzeit?
  - `Init` und `RenderAFrame`
- Der Szenengraph
- Kamera
- Animation
- Ändern von Szeneneigenschaften

## FUSEE installieren und bauen

> #### 👨‍🔧 TODO
>
> - Installiert FUSEE wie im auf der 
>   [FUSEE Installation Wiki Seite](https://github.com/FUSEEProjectTeam/Fusee/wiki/Using-Fusee-in-Visual-Studio-Code) 
>   beschrieben

## Was heißt Echtzeit?

- Bilder müssen so schnell generiert werden, dass eine flüssige
  Animation möglich ist (aktuell > 30 fps)
- Voraussetzung für Interaktion: Benutzereingaben steuern Parameter der
  Bildberechnung, wie z. B. 
  - Position und Orientierung der Kamera im Raum (First Person)
  - Position, Orientierung und Pose von Charakteren (Third Person)

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

Zu Beginn eines Echtzeit 3D-Programmes werden notwendige Initialisierungen vorgenommen,
wie z.B. Laden von 3D-Modellen, Texturen und anderen _Assets_. Aufbau
eines initialen [Szenengraphen](#der-szenengraph).

Dann begibt sich das Programm in eine "Endlos"-Schleife (die nur durch das
Programmende beendet wird). Jeder Schleifendurchlauf erzeugt ein Bild, daher
muss die Schleifenrumpf schnell genug durchlaufen werden können (Zeit
pro Schleifendurchlauf: < 1/30 sec).

Innerhalb dieses Schleifendurchlaufs wird der Status der Eingabegeräte abgefragt,
auf die die Interaktion reagieren soll. Mögliche Eingabegeräte sind z.B.

- Maus (Position, Status der Tasten)
- Tastatur (Status der Tasten - welche sind gedrückt, welche wieder losgelassen)
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
aufgerufen!

Die Datei [Tut08_FirstSteps.cs](Tut08_FirstSteps.cs) enthält minimale Implementierungen
für die beiden Methoden [`Init()`](Tut08_FirstSteps.cs#L22) und 
[`RenderAFrame()`](Tut08_FirstSteps.cs#L29)

> #### 👨‍🔧 TODO
>
> - Öffnet den Ordner `Tut08_FirstSteps` in Visual Studio Code 
> - Öffnet die Debug-Side-Bar und startet "Debug in FUSEE Player" wie
>   auf der FUSEE-Homepage unter 
>   [Build and run the App](https://fusee3d.org/getting-started/firstfuseeapp.html#build-and-run-the-app)
>   beschrieben

Wie man sieht, sieht man nichts - ein Fenster in hellgrün. Das liegt daran, dass
in `Init()` die so genannte _ClearColor_, also die Hintergrundfarbe, mit der
beim Rendern zunächst mal der gesamte zu rendernde Bereich gelöscht wird, auf
hellgrün gesetzt wird. 

In `RenderAFrame()` wird dann für jedes zu rendernde Bild zunächst das gesamte Bild
tatsächlich mit der vorher gesetzten Farbe gelöscht (mit `RC.Clear(...)`) und 
schließlich das im Hintergrund gerenderte Bild, das aus nicht anderem als der 
grünen Fläche besteht, in den sichtbaren Bildbereich gebracht (mit `Present()`).

> #### 👨‍🔧 TODO
>
> - Ändert die Hintergrundfarbe in der [`Init()`-Methode](Tut08_FirstSteps.cs#L25)

## Der Szenengraph

Objekte, die in der Szene sichtbar sein sollen, werden in einem _Szenengraphen_ 
zu einer Szene zusammengestellt. Einen Szenengraphen kann man sich wie die 
Szenenbeschreibung in Blender's 
[Outliner Editor](https://sftp.hs-furtwangen.de/~lochmann/computergrafik2019/script/chapter01/lecture01/#blender-screen-layout)
vorstellen. Das Wort _Graph_ beschreibt den hierarchischen Aufbau, manchmal spricht man auch vom Szenen-_Baum_. Da es in diesen Bäumen vorkommen kann, dass Objekte an mehreren 
Stellen eingehängt sein können, wird der hier mathematisch passendere Begriff _Graph_ verwendet.

In FUSEE besteht ein Szenengraph aus Instanzen der folgenden Datentypen

- [`SceneContainer`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Engine/Core/Scene/SceneContainer.cs#L29)
- [`SceneNode`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Engine/Core/Scene/SceneNode.cs#L31)
- [`SceneComponent`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Engine/Core/Scene/SceneComponent.cs#L9)

Um diese Typen zu verstehen, ist im folgenden Bild ein Beispiel-FUSEE-Szenengraph abgebildet

![FUSEE Szenengraph](_images/SceneHierarchy.png)

Eine Szene beginnt immer mit einem Objekt vom Typ `SceneContainer` (orange). Dieser enthält
eine Liste von Objekten vom Typ `SceneNodeContainer` (gelb). Diese stellen die 
Objekte in der Szene dar. Wie man sehen kann, können Objekte wiederum Kind-Objekte 
enthalten. D.h. jeder `SceneNodeContainer` enthält eine (u.u. leere) Liste, wiederum von 
Objekten vom Typ `SceneNodeContainer`. Die eigentlichen Nutzdaten sind dann in 
`SceneComponentContainer` Objekten (grün) gespeichert. Hier gibt es unterschiedliche
Komponenten-Typen. Die wichtigsten sind

- [`Mesh`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Engine/Core/Scene/Mesh.cs#L10) - enthalten 3D-Geometriedaten wie Punkte, Flächen, Normalen und UVs.
- [`ShaderEffect`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Engine/Core/Scene/ShaderEffect.cs#L114) - enthalten Materialbeschreibungen und Textur-Informationen.
- [`Transform`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Engine/Core/Scene/Transform.cs#L12) - enthalten Positions-, Orientierungs- und Skalierungs-Informationen für die jeweilige Node.

### Ein Würfel

> #### 👨‍🔧 TODO
>
> - Fügt in die Klasse [`Tut08_FirstSteps`](Tut08_FirstSteps.cs#L19) die beiden Felder
>   - `_scene` und
>   - `_sceneRenderer` 
>  ein.

```C#
    public class FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
```
> - Erweitert die Methode [`Init()`](Tut08_FirstSteps.cs#L22) wie folgt, um einen
>   Szenengraphen erzeugt, dessen einziges Objekt ein Würfel ist. Zusätzlich wird 
>   der so genannte `SceneRender` erzeugt, der jede beliebige Szene rendern kann.

```C#
    public override void Init()
    {
        // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
        RC.ClearColor = new float4(1, 1, 1, 1);

        // Create a scene with a cube
        // The three components: one Transform, one ShaderEffect (blue material) and the Mesh
        _cubeTransform = new Transform {Translation = new float3(0, 0, 0)};
        var cubeShader = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue, float4.Zero);
        var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));
        
        // Assemble the cube node containing the three components
        var cubeNode = new SceneNode();
        cubeNode.Components.Add(_cubeTransform);
        cubeNode.Components.Add(cubeShader);
        cubeNode.Components.Add(cubeMesh);

        // Create the scene containing the cube as the only object
        _scene = new SceneContainer();
        _scene.Children.Add(cubeNode);

        // Create a scene renderer holding the scene above
        _sceneRenderer = new SceneRendererForward(_scene);
    }
```
> - Fügt in die Methode [`RenderAFrame()`](Tut08_FirstSteps.cs#L29) den Aufruf
    `_sceneRenderer.Render(RC);` ein. Diese führt dazu, dass jeden Frame der Würfel
    gerendert wird.

```C#
  public override void RenderAFrame()
  {
      SetProjectionAndViewport()

      // Clear the backbuffer
      RC.Clear(ClearFlags.Color | ClearFlags.Depth);

      // Render the scene on the current render context
      _sceneRenderer.Render(RC);

      // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
      Present();
  }
```
> - Lasst das Programm laufen: Es sollte die Seitenansicht eines blauen Würfels
>   auf grünem Untergrund erscheinen.

Um den Code zu verstehen, ist es am besten, ein wenig damit herumzuspielen:

> #### 👨‍🔧 TODO
>
> - Ändert die Farbe des Würfels
> - Ändert Position, Drehung, Skalierung des Würfels
> - Macht aus dem Würfel einen nicht-würfelförmigen Quader. Es gibt zwei Möglichkeiten - welche?
> - Setzt einen Breakpoint auf die Zeile `_sceneRenderer = new SceneRenderer(_scene);` und
>   betrachtet den Inhalt von `_scene` im Watch-Fenster. Verfolgt den hierarchischen Aufbau der 
>   Szene.

## Kamera

Mit oben durchgeführten Änderungen befindet sich der Würfel in einem linkshändigen 
Koordinatensystem, in dem die Y-Achse die Hoch-Achse ist. Die Kamera steht im 
Zentrum des Koordinatensystems und schaut entlang der positiven Z-Achse. Der Würfel 
steht an der Position (0, 0, 50).

> #### 👨‍🔧 TODO
>
> - Führt Euch die Situation vor Augen. Zeichnet ein Bild der Szene mit Koordinatenachseen,
>   Kamera-Position und Kamera-Blickrichtung.

In vielen Anwendungen soll die Kamera in der Szene bewegt werden. Dazu muss in FUSEE die so
genannte _View-Matrix_ verändert werden. Glücklicherweise lässt sich die View-Matrix mit einigen 
sehr komfortablen Methoden ändern, ohne dass sehr tief in die Matrix-Rechnung eingetaucht werden
muss.

Im Folgenden soll der Würfel in die Mitte der Szene gebracht werden und die Kamera von schräg
hinten auf die Szene schauen.

> #### 👨‍🔧 TODO
>
> - Ändert die Transform-Komponente des Würfels so ab, dass dieser nun im Zentrum des
>   Koordinatensystems steht ((0, 0, 0) statt (0, 0, 50).
>    
> ```C#
> var cubeTransform = new Transform {Scale = new float3(1, 1, 1), Translation = new float3(0, 0, 0)};
> ```
> - Fügt VOR dem Rendern der Szene Code ein, der die View-Matrix des RenderContext verändert,
>   und somit die Kamera platziert und rotiert:

```C#
  public override void RenderAFrame()
  {
      SetProjectionAndViewport();

      // Clear the backbuffer
      RC.Clear(ClearFlags.Color | ClearFlags.Depth);

      // Setup the camera 
      RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(0.2f);

      // Render the scene on the current render context
      _sceneRenderer.Render(RC);

      // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
      Present();
  }
```

Nun erscheint der Würfel leicht gedreht. Die View-Matrix enthält die Transformationen der Kamera
aus ihrer Lage im Zentrum des Koordinatensystems mit Blickrichtung in positive Z-Achse an
ihre Ziel-Position und -Orientierung. **Dies allerdings in umgekehrter Reihenfolge und in umgekehrtem Koordinaten-Sinn**. Die Anweisung ``float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(0.2f);`` erzeugt also eine Kamera-Transformation, die die Kamera 
ZUERST um -50 Einheiten entlang der Z-Achse bewegt UND DANN um -0.2 Radiant um die (Welt-)Y-Achse dreht.

> #### 👨‍🔧 TODO
>
> - Auch das ist nur mit einer Skizze zu verstehen. Zeichnet ein Bild der Szene mit 
>   Koordinatenachseen, Kamera-Position und Kamera-Blickrichtung und den beiden 
>   Transformationen.

## Animation

Nun soll sich die Kamera um den Würfel drehen. Dazu muss in `RenderAFrame()` der aktuelle
Drehwinkel für jedes Bild abgeändert werden.

> #### 👨‍🔧 TODO
>
> - Fügt der Klasse `FirstSteps` ein weiteres Feld hinzu, das den aktuellen Drehwinkel
>   der Kamera in Radiant enthält und initialisiert den Winkel mit 0.
>  ```C#
>   private float _camAngle = 0;
>  ``` 
> - Ändert in der Methode `RenderAFrame()` das Setzen der Kamera-Matrix folgendermaßen ab:
>  ```C#
>   // Animate the camera angle
>   _camAngle = _camAngle + 0.01f;
>
>   // Setup the camera 
>   RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle);
>  ``` 
>
>  - Lasst Euch den aktuellen Drehwinkel mit der Methode `Diagnistics.Log()` auf der 
>    Debug Console von Visual Studio Code ausgeben.

Erstellen und Laufen lassen sollte nun den Würfel mit einer animierten Kamera zeigen, die sich
um den Würfel herum dreht.

### Unabhängigkeit von der Frame-Rate 

Die so erstellte Animation erhöht den Drehwinkel in jedem gerenderten Bild um einen konstanten
Wert (0.01 Radiant). Das führt auf unterschiedlichen Rechnern zu unterschiedlich schnellen 
Animationen: Auf leistungsstarken Rechnern, die viele Frames pro Sekunde berechnen können,
läuft die Animation schnell. Auf schwächeren Rechnern, die wenig Frames pro Sekunde berechnen 
können, dreht sich der Würfel langsamer.

Auch auf ein- und dem selben Rechner kann die Animation zu unterschiedlichen Zeiten unterschiedlich
schnell dargestellt werden, z.B. wenn der Rechner auf Grund von anderen, gleichzeitig laufenden
Prozessen stark beansprucht wird.

Um die Animation unabhängig von der aktuellen Frame-Rate zu machen, können alle
Werte, die Geschwindigkeiten repräsentieren, mit der so genannten _Delta-Time_ skaliert werden.

Dabei handelt es sich um die Zeit, die seit dem Rendern des letzten Frame vergangen ist. In 
Fusee kann über die Eigenschaft `DeltaTime` der statischen Klasse `Time` auf diesen Wert,
gemessen in Sekunden, zugegriffen werden. Da in Echtzeit-3D-Applikationen das Rendern eines Frames 
sehr schnell gehen muss (meistens im Bereich 1/30 Sekunde oder schneller), ist dieser Wert sehr klein.

Werden Geschwindigkeiten (Inkremente) mit `DeltaTime` skaliert, ändert sich deren "Einheit: Statt
in Wertänderung-pro-Frame gibt der Wert nun die Wertänderung-pro-Sekunde an. Dadurch ergeben sich 
größere Werte.

> #### 👨‍🔧 TODO
>
> - Ändert den Befehl, der pro Frame die aktuelle Kamera-Drehung berechnet, wie folgt:
> ```C#
>   // Animate the camera angle
>   _camAngle = _camAngle + 90.0f * M.Pi/180.0f * DeltaTime;
> ```

Dadurch dreht sich der Würfel exakt mit einer Vierteldrehung (= 90°) pro Sekunde, egal
mit wieviel Frames die Animation läuft.

## Ändern von Szenen-Eigenschaften

Um die Eigenschaften von Objekten wie z.B. Positionen, Skalierungen, Rotation und Farben
während des Renderns zu ändern, müssen Referenzen auf die jeweiligen Komponenten gespeichert
werden. Im Folgenden Beispiel soll der Würfel in Abhängigkeit von der vergangenen Zeit
sinus-förmige Auf- und Ab-bewegungen durchführen

> #### 👨‍🔧 TODO
>
> - Fügt in die Klasse `FirstSteps` ein Feld ein, das die Transform-Komponente des Würfels
>   speichern kann, so dass wir nicht nur aus `Init()`, sondern auch aus `RenderAFrame()`
>   darauf zugreifen können.
>
> ```C#
>  private Transform _cubeTransform;
> ```
>
> - Beim Erzeugen der Szene in `Init()`, ersetzt die lokale Variable `cubeTransform` durch
>   das neu angelegte Feld `_cubeTransform`.
>
> ```C#
>   _cubeTransform = new Transform {Scale = new float3(1, 1, 1), Translation = new float3(0, 0, 0)};
>  ...
>  cubeNode.Components.Add(_cubeTransform);
> ```
>
> - In `RenderAFrame()` ändert die Position des Würfels in der Transform-Komponente als 
>   Funktion der Zeit. Da wir diesmal die Zeit seit Beginn der Applikation (und nicht die
>   Delta-Zeit seit dem letzten Frame) verwenden wollen, greifen wir auf `TimeSinceStart` zu.
>
> ```C#
>  // Animate the cube
>  _cubeTransform.Translation = new float3(0, 5 * M.Sin(3 * TimeSinceStart), 0);
> ```
>
> - Verdeutlicht Euch die Wirkung der Faktoren 5 und 3 für Amplitude und Frequenz der 
>   Bewegung, in dem Ihr die Werte ändert.


## Aufgabe

- Fügt mehrere Würfel in der Init-Methode in Eure Szene ein!
- Animiert weitere Eigenschaften wie z.B. Positionen, Skalierungen, Rotationen und Farben!
- Eventuell können auch mit Schleifen und Berechnungen sehr viele Würfel eingefügt werden.
- Um die Eigenschaften vieler Objekte zu verändern, können Referenzen auf deren Komponenten
  in Arrays oder Listen Feldern gehalten werden und ebenfalls in Schleifen verändert werden. 












