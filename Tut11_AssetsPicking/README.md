# Assets & Picking

## Lernziele

- Mit Blender 3D-Modelle erstellen und in FUSEE darstellen
- Auf Einzelteile einer 3D-Szene zugreifen und verändern
- Suchen nach Namen in Szenengraphen
- Picking: Auf Objekte klicken

## Voraussetzung: FUSEE-Export-AddOn für Blender

In diesem Kapitel sollen 3D-Geometrien (Meshes) nicht mehr durch Code erzeugt werden, sondern als Modelle,
die mit Blender erstellt wurden, geladen werden können. 

Hierarchien im bereits bekannten FUSEE-Szenengraphen-Format (bestehend aus Nodes und Komponenten)
können als `.fus`-Dateien gespeichert und geladen werden. Um mit Blender erstellte 3D-Modelle als .fus-Datei 
zur Verwendung in FUSEE-Applikationen zu verwenden, muss das FUSEE-Export-AddOn für Blender installiert sein.

Die Installation des FUSEE-Export-AddOn für Blender ist auf der [FUSEE-Installationsseite](https://fusee3d.org/getting-started/install-fusee.html#installenable-the-fusee-blender-add-on-within-blender) beschrieben. 


### Features des FUSEE-Exporters

Mit dem FUSEE-Export-AddOn für Blender sich lassen einige Features, die in Blender möglich sind,
als FUSEE-Inhalte exportieren. Welche das genau sind, ist im [FUSEE Wiki](https://github.com/FUSEEProjectTeam/Fusee/wiki/FUSEE-Exporter-Blender-Add-on#exported-features) beschrieben - hier eine kurze Zusammenfassung: 
  
  - Eltern-Kind-Verhältnisse von Blender-Objekten als Hierarchien von `SceneNode`-Instanzen.
  - Die Namen der Blender-Objekte als `Name`-Eigenschaft des jeweiligen `SceneNode`-Objektes
  - 3D-Geometrien als `Mesh`
    - Eckpunkt-Positionen (`Vertices`)
    - Normalen (`Normals`) in Abhängigkeit der "Smooth / Flat"-Einstellung
    - Textur-Koordinaten (`UVs`)
    - Flächen aufgeteilt in Dreiecke (`Triangles`)
  - Position, Rotation und Skalierung jeweils relativ zu den Eltern-Einstellungen und 
    zum Koordinaten-Ursprung (Pivot-Point) als `Transform`
  - Farben aus den Blender-Material-Einstellungen für Principled BSDF oder  Diffuse BSDF Nodes


## Modelle als Assets

Inhalte, die nicht durch Programmierung erstellt sind, heißen in 3D-Echtzeit-Umgebungen
(Game-Engines) meist _Assets_. In einer FUSEE-Applikation können Assets in Form von 
als .fus-Datei expotierten Blender-3D-Szenen folgendermaßen eingebunden werden.

> #### 👨‍🔧 TODO
> 
> - Erzeugt eine Szene in Blender mit folgenden Features
>
>   - Mindestens zwei Objekte mit selbst-vergebenen Namen
>   - Eltern-Kind-Verhältnisse zwischen den Objekten
>   - Materialien ***entweder*** mit Diffuse- und Glossy-BSDF-Nodes ***oder*** mit dem 
>     Principled-BSDF-Shader im Cycles-Renderer
>
> - Exportiert die Szene als .fus-Datei
>
> - Kopiert die .fus-Datei in den "Assets" Unterordner Eures FUSEE-Projektes
>

Auf derart hinzugefügte Assets kann in einer FUSEE-Applikation dann mit der Methode
[`AssetStorage.Get<>()`](https://github.com/FUSEEProjectTeam/Fusee/blob/develop/src/Base/Core/AssetStorage.cs#L49)
zugegriffen werden. Der Methode muss dabei in Spitzklammern (`<>`) der Typ und 
in der Methodenparameterliste (`()`) der Name übergeben werden. 

In unserem Fall enthält eine .fus-Datei immer ein Objekt vom Typ `SceneContainer`. 
Der Name des Assets entspricht dem Dateinamen (MIT Dateinamenerweiterung).

> #### 👨‍🔧 TODO
> 
> - Ersetzt in der `Init`-Methode den Aufruf von 
>   ```C#
>     _scene = CreateScene();
>   ```
>   durch
>   ```C#
>     _scene = AssetStorage.Get<SceneContainer>("CubeCar.fus");
>   ```
>   Ersetzt dabei ggf. den Namen "CubeCar.fus" durch den Namen Eurer eigenen Datei.
>
> - Da nun `CreateScene()` nicht mehr aufgerufen wird, wird auch `_baseTransform` nicht
>   mehr initialisiert. Kommentiert fürs Erste die Zeile
>
>   ```C#
>     // _baseTransform.Rotation = new float3(0, M.MinAngle(TimeSinceStart), 0);
>   ```
>   einfach aus (`//` voranstellen).
>   


Nun besteht die Szene aus dem Inhalt der in Blender erzeugten `.fus`-Datei. 

![Blender neben FUSEE](_images/BlenderFusee.png)

Um näher zu verstehen, wie das FUSEE-Export-AddOn für Blender die Szene erzeugt,
schauen wir uns den Inhalt der .fus-Datei nach dem Laden in der Applikation an.

> #### 👨‍🔧 TODO
> 
> - Setzt einen Breakpoint HINTER die Zeile, in der das Modell geladen wird.
>   ```C#
>     _scene = AssetStorage.Get<SceneContainer>("CubeCar.fus");
>   ```
>
> - Betrachtet den Inhalt von `_scene` im Watch-Fenster und klappt die Hierarchie auf.
> - Vergleicht die Hierarchie mit der im "Outliner"-Editor von Blender
>
> ![Blender neben FUSEE](_images/Hierarchies.png)
>
> - Seht Euch die Inhalte der Komponenten an
> - Zeichnet eine Skizze Eurer Szene in 
>   der Scene-Node-Components-Notation aus [Kapitel 08 - Der Szenengraph](https://github.com/griestopf/ComputergrafikMIB/tree/master/Tut08_FirstSteps#der-szenengraph)
>   

Um unser Objekt nun interaktiv zu verändern, z.B. Farben, Positionen, Rotationen, wollen wir
auf einzelne Komponenten zugreifen können.

Da wir die Szene nicht mehr im Code selbst erzeugen, müssen wir die Komponenten, auf die 
wir zugreifen wollen, suchen. Das können wir entweder, in dem wir unser Wissen über die 
Hierarchie ausnutzen, um dann z.B. auf das dritte Enkel-Objekt im ersten Kind des fünften
Objektes in der Szene zuzugreifen. 

Einfacher ist es aber, die Objekte über deren Namen zu identifizieren und nach dem Laden 
der Szene einfach die Komponenten in den Objekten, deren Namen wir kennen zu suchen.

#### Ändern von Position/Rotation/Orientierung
Um Beispielsweise auf die Transformkomponente des rechten Hinterrades in o.a. Beispielszene
zuzugreifen, kann diese über folgenden Aufruf in der Szene gesucht und im Feld `_rightRearTransform` abgespeichert werden:

```C#
  private Transform _rightRearTransform;
...
  _rightRearTransform = _scene.Children.FindNodes(node => node.Name == "RightRearWheel")?.FirstOrDefault()?.GetTransform();
```

Die in einer Zeile zusammengesetzte Anweisung besagt in etwa:

- Durchkämme die Hierarchie sämtlicher Objekte in der Szene (`_scene.Children.FindNodes`).
- Suche dabei nach Nodes deren Name "RightRearWheel" lautet (`node => node.Name == "RightRearWheel"`)
- Von den so gefundenen Nodes nimm die erste, falls eine existiert (`.FirstOrDefault()`).
- Liefere die dort enthaltene Transform-Komponente (`.GetTransform()`)

Die seltsam anmutenden "`?.`" Operatoren heißen übrigens _Elvis-Operator_ (warum wohl?) und bedeuten,
dass nur auf das im vorangestellten Aufruf zurückgelieferte Objekt zugegriffen werden soll,
falls auch tatsächlich eines existiert, ansonsten soll `null` zurückgegeben werden.
Falls also gar kein Objekt gefunden wurde, 
das den gesuchten Namen trägt ODER ein Objekt gefunden wurde, dieses aber keine Transform-Komponente 
enthält, resultiert der gesamte Aufruf darin, dass `_rightRearTransform` den Wert `null` zugewiesen
bekommt und nicht etwa in einem Absturz, weil versucht wurde, in einem nicht vorhandenen Objekt eine
Transform-Komponente zu suchen.


#### Ändern von Farben

Wurde einem Objekt in Blender ein Material zugewiesen, besitzt dieses beim Export über den FUS-Exporter beim Einlesen in FUSEE eine `SurfaceEffect` Komponente. Über diese lassen sich die farbgebenden Parameter wie z.B. die Diffuse-Farbe ändern:

```C#
  private SurfaceEffect _rightRearEffect;
...
  _rightRearEffect = _scene.Children.FindNodes(node => node.Name == "RightRearWheel")?.FirstOrDefault()?.GetComponent<SurfaceEffect>();
  _rightRearEffect.SurfaceInput.Albedo = (float4) ColorUint.OrangeRed;

```

> #### 👨‍🔧 TODO
>
> - Sucht nach oben angegebenem Muster ein vorhandenes Objekt in der geladenen FUSEE-Szene nach dessen Namen.
> - Speichert eine Referenz auf die `Transform`-Komponente (`GetTransform()`) und die `SurfaceEffect`-Komponente (`GetComponent<SurfaceEffect>()`) des Objektes
> - Animiert die Rotation des Objektes und die Farbe des Objektes innerhalb von `RenderAFrame()`.


## Picking

Eine häufig vorkommende Aufgabe in Echtzeit-3D-Anwendungen ist es, herauszufinden, welche Objekte in 
der 3D-Szene an unter einer bestimmten 2D-Pixelposition auf dem Bildschirm liegt, beispielsweise
dort, wo ein Benutzer gerade mit der Maus hingeklickt oder mit dem Finger eine Touch-Geste vollführt
hat. FUSEE bietet hierzu die Klasse 
[`SceneRayCaster`](https://github.com/FUSEEProjectTeam/Fusee/blob/master/src/Engine/Core/SceneRayCaster.cs#L99)
mit deren Hilfe diese Aufgabe bewerkstelligt werden kann. Wie der Name der Klasse sagt, wird ein Strahl (Ray) in die Szene geschossen und sämtliche Auftreffpunkte mit Objekten in der Szene werden als `RayCastResult` in einer Liste zurückgegeben. Dabei kann der Strahl, der in die Szene geschossen wird, einfach als Pixel-Position des Ausgabefensters angegeben werden. Der RayCaster generiert dann selbständig einen Strahl aus der Kameraposition durch das angegebene Pixel in die Szene.

Wie der `SceneRenderer` und auch die weiter oben beschriebene `FindNodes()` Methode wird beim Picking
eine Traversierung des Szenengraphs durchgeführt, d.h. alle Nodes und alle notwendigen Komponenten
werden besucht. Während beim Rendern der Besuch dazu führt, dass jede Komponente ihren Beitrag am
zu rendernden Bild leistet und beim Suchen beim Besuch ein Suchkriterium überprüft wird, werden beim 
RayCast jedes Dreieck von jedem Objekt darauf hin untersucht, ob es vom Strahl getroffen wird.

Wann immer dieser Test positiv ist, werden eine Reihe von Informationen gesammelt,
die dann vom Benutzer ausgewertet werden können. Zu diesen Informationen gehört:

- Die gerade traversierte Node
- Die gerade traversierte (Mesh-)Komponente
- Der Index des ersten Punktes des Dreiecks in der `Triangles`-Liste, für das der Punkt-im-Dreieck-Test
  positiv war
- Die so genannten baryzentrischen Koordinaten, die angeben wo exakt innerhalb des Dreiecks der 
  Punkt liegt
- Modell-, View- und Projektionsmatrix, mit denen die Transformation der Modell-Koordinaten in 
  Bildschirmkoordinaten stattfand.

Diese Informationen sind in der Klasse
[`RayCastResult`](https://github.com/FUSEEProjectTeam/Fusee/blob/master/src/Engine/Core/SceneRayCaster.cs#L13)
zusammengefasst.

Mit diesen Informationen lassen sich nicht nur die unter einem Bildschirm-Pixel liegenden 3D-Objekte
identifizieren, diese lassen sich auch entlang der z-Koordinate sortieren, so dass z.B. das am weitesten
vorne liegende Objekt herausgefunden werden kann. Zudem kann auch das Dreieck identifiziert werden, das
getroffen wurde, sowie die exakte Position des "Auftreffpunktes" errechnet werden und zwar in Modell-
Welt- oder Bildschirmkoordinaten.

> #### 👨‍🔧 TODO
>
> - Erzeugt eine Klassenvariable `private SceneRayCaster _sceneRayCaster` (analog zum `SceneRenderer`) 
>   und fügt folgenden Code in die Methode `InitAsync()` _nach_ dem Laden der Szene ein:
>   ```C#
>     _scenePicker = new ScenePicker(_scene);
>   ```
> - Fügt folgenden Code in die Methode `RenderAFrame()` ein, NACHDEM die Kamera gesetzt wurde .
>   ```C#
>           _camTransform.RotateAround(float3.Zero, new float3(0, Keyboard.LeftRightAxis * DeltaTime, 0));
>
>            if (Mouse.LeftButton)
>            {
>                float2 pickPos = Mouse.Position;
>
>                RayCastResult newPick = _sceneRayCaster.RayPick(RC, pickPos).OrderBy(rr => rr.DistanceFromOrigin).FirstOrDefault();
>
>                if (newPick?.Node != _currentPick?.Node)
>                {
>                    if (_currentPick != null)
>                    {
>                        var ef = _currentPick.Node.GetComponent<SurfaceEffect>();
>                        ef.SurfaceInput.Albedo = _oldColor;
>                    }
>                    if (newPick != null)
>                    {
>                        var ef = newPick.Node.GetComponent<SurfaceEffect>();
>                        _oldColor = ef.SurfaceInput.Albedo;
>                        ef.SurfaceInput.Albedo = (float4) ColorUint.OrangeRed;
>                    }
>                    _currentPick = newPick;
>                }
>            }
>   ```
>
>
> - Überprüft die Lauffähigkeit, indem Ihr die Applikation startet und auf unterschiedliche
>   Objekte Eurer 3D-Szene klickt. Es müssten jeweils die angeklickten Einzelteile durch die
>   Highlight-Farbe `ColorUint.OrangeRed` gekennzeichnet werden.

## Aufgabe

Erstellt ein eigenes 3D-Modell in Blender mit folgenden Anforderungen:

- Ein Fahrzeug mit (mind.) vier Rädern
- Die Räder sollen so strukturiert sein, dass eine Drehung sichtbar ist, also z.B. ein stilisiertes
  Reifenprofil oder Speichern enthalten
- Das Chassis soll einen beweglichen Aufbau enthalten. Mögliche Fahrzeuge sind somit
  - Gabelstapler (Gabel heb- und drehbar)
  - Bagger (Aufbau drehbar, Arm über mehrere Achsen beweglich)
  - Panzer (Kanone dreh und schwenkbar)
  - Mars-Rover (beweglicher Greifarm)
  - ...
- Der Aufbau soll über mindestens zwei hierarchisch in Eltern-Kind-Beziehung stehende Achsen beweglich sein
- Die Hierarchie muss so aufgebaut sein, dass sich durch Drehungen oder Positionsänderungen sinnvolle
  Animationen erzeugen lassen.

Mit diesem Modell soll dann eine erste Applikation erzeugt werden

- Ladet das Modell ein eine eigene FUSEE-Applikation und sucht die relevanten Komponenten mit `FindNode()` wie oben.
  Speichert die Komponenten in Feldern, so dass aus `RenderAFrame()` darauf zugegriffen werden kann.
- Erzeugt eine Interaktion, die
  - Den Benutzer über Maus-Klicks das zu bewegende Teil auswählen lässt
  - Die Farbe des gerade selektierten Teils verändert
  - Pfeil- oder WASD- Eingaben (oder Teile davon) auf Bewegungen der Achsen des gerade selektierten (Teil-)Objektes legt







