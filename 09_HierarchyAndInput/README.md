# Hierarchy & Input

## Lernziele

- Hierarchien und Transformationen verstehen
  - Eltern vererben ihre Transformation auf Kinder
  - Was ist der Pivot-Point und wie setze ich diesen
- Eingabe
  - Tastatur
  - Maus
  - Achsen

## Hierarchien

### Szenengraph in einer Anweisung

Die Solution HierarchyInput.sln enthält eine leicht abgeänderte Version des letzte
Standes. 

> **TODO**
>
> - Öffnet die Solution (HierarchyInput.sln) in Visual Studio, 
>   baut die Desktop-Version und lasst sie laufen.
> - Öffnet die Source-Code-Datei HierarchyInput.cs und betrachtet die Methoden
>   `Init()` und `RenderAFrame`
> - Identifiziert Änderungen zur letzten Übung.

Wie in der  Lektion 08 wird eine Szene, die nur aus einem Cuboid-Objekt (Quader)
besteht, erzeugt und gerendert. Allerdings ist der Quader nun grau und nicht mehr nicht würfelförmig.

Was hat sich noch gegenüber der letzten Lektion 08 geändert?: 

- Sämtlicher Animations-Code aus `RenderAFrame()` ist verschwunden.
- `Init()` sieht aufgeräumter aus: Das liegt daran, dass die Szene nun 
  in einer eigenen Methode namens `CreateScene()` erzeugt wird.
- Die Kamera wurde entlang der Hoch-Achse (Y) um 10 Einheiten nach oben verschoben.

Aber auch das Zusammensetzen der Szene aus Node und Komponenten innerhalb der Methode
`CreateScene()` sieht anders aus als beim letzten mal. Der Code, der die Szene erzeugt,
besteht nur aus einer einzelnen Anweisung (die sich allerding über mehrere Zeilen
erstreckt):

```C#
    return new SceneContainer
    {
        Children = new List<SceneNodeContainer>
        {
            new SceneNodeContainer
            {
                Components = new List<SceneComponentContainer>
                {
                    // TRANSFROM COMPONENT
                    _baseTransform,

                    // MATERIAL COMPONENT
                    new MaterialComponent
                    {
                        Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) },
                        Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                    },

                    // MESH COMPONENT
                    SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                }
            }
        }
    };
```

Möglich ist das durch ein Feature von C#, Eigenschaften ("Klassenvariablen"), Objekte, 
Arrays, Listen und andere so genannte Container-Klassen mit einfachen Anweisungen zu 
initialisieren. Das Ergebnis sieht fast ein bisschen wie eine JSON Datei aus - ein durchaus gewünschter Effekt.
Zumindest lässt mit dieser Schreibweise die hierarchische Struktur der Szene viel besser
erkennen, als im letzten Beispiel.

> **TODO**
>
> - Zeichnet den Szenengraphen auf, der durch o.a. Code erzeugt wird. Verwendet dazu die im Unterricht verwendete 
>   Darstellung: 
> 
>   - Szene: Orangenes Quadrat
>   - Nodes: Gelbe Quadrate
>   - Komponenten: grüne abgerundete Quadrate
>   - Komponentenlisten horizontal angeordnet
>   - Kind-Listen vertikal angeordnet
>
> - Überprüft, ob Eure Annahme stimmt, in dem Ihr einen Breakpoint _hinter_ den Aufruf von `CreateScene()`
>   setzt und im Watch-Fenster des Debugger die Hierarchie anschaut.
>
> - Lest den [Abschnitt im C# Programmierhandbuch, der die Objekt- und Auflistungsinitialisierer
>   erklärt](https://docs.microsoft.com/de-de/dotnet/articles/csharp/programming-guide/classes-and-structs/object-and-collection-initializers). Verdeutlich Euch, wie die Liste der drei Komponenten im vorigen Beispiel über Hinterinanderausführungen der
>   `Add()`-Anweisungen aufgebaut wurde und wie diese nun über eine durch Komma getrennte Aufzählung realisiert wird.
>

### Mehr Objekte

Es soll nun Zug um Zug ein Modell aufgebaut werden, dass so aussieht, wie der 
[Roboterarm aus der ersten Lektion](https://sftp.hs-furtwangen.de/~mch/computergrafik/script/chapter01/lecture01/#3-hierarchien):

![Cuboter](_images/Robot.png)

Die Einzelteile des Roboters sollen im Folgenden einheitlich benannt werden:

- Graue Bodenplatte: `Base`
- Rote Säule: `Body`
- Grüner Oberarm: `UpperArm`
- Blauer Unterarm: `ForeArm`

Der Roboter soll zunächst so aufgebaut werden, dass alle Arme nach oben zeigen. Die kurzen Kantenlängen sollen jeweils zwei Einheiten
betragen, die langen Kanten sollen zehn Einheiten messen. Die Arme sollen sich jeweils zwei Einheiten überlappen, so dass die
"Gelenke" an jedem Arm jeweils eine Einheit nach innen ragen. Folgende Skizze soll dabei behilflich sein:

![Cuboter Zeichnung](_images/RoboBlueprint.png)

> **TODO**
>
> - Erzeugt ein weiteres Objekt (Node) im Szenengraphen, das aus einem roten länglichen Quader der Dimension (2, 10, 2)
>   besteht, der in der Mitte auf dem grauen Quader steht. Dazu muss
>   
>   - Ein neues Feld (Klassenvariable) für die Transformationskomponente eingefügt werden
>     (`TransformComponent _bodyTransform`)
>   - Ein zweiter mit `new` erzugter `SceneNodeContainer` in die `Children` Liste der Szene eingefügt werden, der
>     wiederum drei Komponenten enthält.
>
> - Versucht zunächst selbst die Stellen im o.a. Code zu finden, wo neue Stellen einzufügen sind. Falls es nicht klappt,
>   verwendet folgenden Code:
>

```C#
    private TransformComponent _baseTransform;
    private TransformComponent _bodyTransform;

    SceneContainer CreateScene()
    {
        // Initialize transform components that need to be changed inside "RenderAFrame"
        _baseTransform = new TransformComponent
        {
            Rotation = new float3(0, 0, 0),
            Scale = new float3(1, 1, 1),
            Translation = new float3(0, 0, 0)
        };
        _bodyTransform = new TransformComponent
        {
            Rotation = new float3(0, 0, 0),
            Scale = new float3(1, 1, 1),
            Translation = new float3(0, 6, 0)
        };

        // Setup the scene graph
        return new SceneContainer
        {
            Children = new List<SceneNodeContainer>
            {
                // GREY BASE
                new SceneNodeContainer
                {
                    Components = new List<SceneComponentContainer>
                    {
                        // TRANSFROM COMPONENT
                        _baseTransform,

                        // MATERIAL COMPONENT
                        new MaterialComponent
                        {
                            Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) },
                            Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                        },

                        // MESH COMPONENT
                        SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                    }
                },
                // RED BODY
                new SceneNodeContainer
                {
                    Components = new List<SceneComponentContainer>
                    {
                        _bodyTransform,
                        new MaterialComponent
                        {
                            Diffuse = new MatChannelContainer { Color = new float3(1, 0, 0) },
                            Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                        },
                        SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                    }
                }
            }
        };
    }
```

> **TODO**
>
> - Wer o.g. Code kopiert hat, sollte folgende Fragen bantworten können:
>
>   - Wo wird die Farbe für den roten body festgelegt? 
>   - Wo wird die Position für den roten Cuboid festgelegt?
>   - Warum ist dort festgelegt, dass der rote Cuboid 6 Einheiten entlang der Y-Achse transliert werden soll?
>   

## Kindeskinder

Nun soll der grüne Oberarm (`UpperArm`) folgen. Dieser könnte nun als weiteres Kind in die Szenenliste eingefügt werden.
Später wollen wir aber den Roboter bewegen. Dabei soll der grüne Arm allen Bewegungen der roten Säule (`Body`) folgen.
Wie bereits in Blender kann dieses Verhalten durch Eltern-Kind-Beziehungen erreicht werden. Solche können wir aufbauen,
weil alle `SceneNodeContainer`-Objekte ebenfalls die MÖglichkeit haben, eine `Children`-Liste zu enthalten. 

Somit sollten wir den neu einzufügenden grünen Oberarm nicht als drittes Kind in die Szenen-Liste einfügen, sondern als 
(einziges) Kind der roten Säule. Folgender Code zeigt wie es geht: 

```C#
    // RED BODY
    new SceneNodeContainer
    {
        Components = new List<SceneComponentContainer>
        {
            _bodyTransform,
            new MaterialComponent
            {
                Diffuse = new MatChannelContainer { Color = new float3(1, 0, 0) },
                Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
            },
            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
        },
        Children = new List<SceneNodeContainer>
        {
            // GREEN UPPER ARM
            new SceneNodeContainer
            {
                Components = new List<SceneComponentContainer>
                {
                    _upperArmTransform,
                    new MaterialComponent
                    {
                        Diffuse = new MatChannelContainer { Color = new float3(0, 1, 0) },
                        Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                    },
                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                },
            }
        }
    }
```

> **TODO**
>
> - Fügt mit Hife des obenstehenden Code den grünen Arm als Kind der roten Säule hinzu.
> - erzeugt die `_upperArmTransform`-Komponente analog zu den beiden anderen Transform-Komponenten
> - Setzt die Koordinaten des `Translation`-Feldes der Transform-Komponte so, dass der grüne Arm
>   exakt wie in o.a. Skizze und in folgdendem Screenshot erscheint.
>
>   ![Grüner Arm](_images/GreenArm.png)
> 
> - Erklärt Euch anhand der Skizze, wie diese Koordinaten zustande kommen.

## Pivot Point

Da nun der grüne Oberarm ein Kind der roten Säule ist, müsste dieser ja alle Bewegungen des roten Armes 
mitmachen. Das wollen wir ausprobierern

> **TODO**
> 
> - Rotiert die rote Säule (`Body`) ein wenig um die Y-Achse, indem Ihr deren Transform-Komponente verändert:
>
>   ```C#
>      _bodyTransform = new TransformComponent
>      {
>          Rotation = new float3(0, 0.2f, 0),
>          Scale = new float3(1, 1, 1),
>          Translation = new float3(0, 6, 0)
>      };
>   ```
>
> - Um wieviel Grad wird mit o. a. Transform-Komponente die rote Säule um Y rotiert?
>

Als Ergebnis müsste die Säule jetzt inklusive dem daran hängenden Unterarm rotiert sein. Nun soll sich der grüne
Arm um seine lokale X-Achse gegenüber der roten Säule verdrehen lassen.

> **TODO**
> 
> - Rotiert den grünen Arm (`UpperArm`) ungefähr 90° um die X-Achse, indem Ihr dessen Transform-Komponente verändert:
>
>   ```C#
>      _upperArmTransform = new TransformComponent
>      {
>          Rotation = new float3(1.5f, 0, 0),
>          Scale = new float3(1, 1, 1),
>          Translation = new float3(2, 8, 0)
>      };
>   ```
>
> - Warum ist die Angabe 1,5f ungefähr 90°?

Ergebnis? Der Roboter sieht ziemlich kapputt aus:

![Grüner Arm Futsch](_images/RobotBroken.png)

Der Grüne Arm scheint aus dem Gelenk gesprungen zu sein. Das lieg daran, dass der Koordinatenursprung des Cuboid-Körpers
immer in der Mitte des Quaders liegt. Sämtliche Transformationen in der `TransformComponent` beziehen sich auf
den Ursprung, so auch die Rotation. Das Rotationszentrum eines Körpers bezeichnet man auch mit ***Pivot Point***.

Notiz am Rande: Mit dem `Translation`-Feld (`Translation = new float3(2, 8, 0)`) haben wir die Mitte des grünen
Quaders so weit nach oben geschoben, bis wir die gewünschte Position erreicht haben. 

Wir wollen nun den Pivot Point verändern. Das können wir, indem wir eine weitere Ebene in unsere Hierachie einfügen, 
die allerdings kein Mesh (und auch kein Material enthält). Ausgehend von der Mitte der roten Säule schieben wir unser
Koordinatensystem mit Hilfe eines "leeren" `SceneNodeContainer` zunächst so weit nach oben, dass der Ursprung des 
neuen Koordinatensystem im Scharnier zwischen Grün und Rot liegt. In diese Node fügen wir dann eine weitere Child-Node
ein, die die eigentliche Geometrie enthält und diese an die richtige (relative) Position schiebt.

> **TODO**
>
> - Schreibt den Teil des Szenengraphen für den grünen Oberarm so um, dass dieser nun aus zwei Hierarchiestufen
>   besteht: Einer äußeren Node für den _Pivot Point_ und einer inneren Node für die Geometrie.
>
>   ```C#
>    // GREEN UPPER ARM
>    new SceneNodeContainer
>    {
>        Components = new List<SceneComponentContainer>
>        {
>            _upperArmTransform,
>        },
>        Children = new List<SceneNodeContainer>
>        {
>            new SceneNodeContainer
>            {
>                Components = new List<SceneComponentContainer>
>                {
>                    new TransformComponent
>                    {
>                        Rotation = new float3(0, 0, 0),
>                        Scale = new float3(1, 1, 1),
>                        Translation = new float3(0, 4, 0)
>                    },
>                    new MaterialComponent
>                    {
>                        Diffuse = new MatChannelContainer { Color = new float3(0, 1, 0) },
>                        Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
>                    },
>                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
>                }
>            }
>        }
>    }
>   ```
>
> - Verändert die `TransformComponent`für den grünen Upper Arm, so dass dessen Pivot Point nun auf der Y-Achse
>   des Welt-Koordinatensystems bei 10 zum liegen kommt. Gemessen von der Mitte der roten Säule (die ja das 
>   Eltern-Objekt ist und daher der Ursprung des lokalen Koordinatensystems für dessen Kinder), sind das
>   4 Einheiten nach oben
>
>   ```C#
>   _upperArmTransform = new TransformComponent
>   {
>       Rotation = new float3(1.5f, 0, 0),
>       Scale = new float3(1, 1, 1),
>       Translation = new float3(2, 4, 0)
>   };
>   ```

Damit sollten die Bestandteile des Roboters (noch immer ohne blauen Unterarm) mit beliebigen Dreheungen
der roten Säule um die Y-Achse und des grünen Arms um die X-Achse glaubhaft aussehen, ohne dass die 
Einzelteile auseinander gerissen werden.

Schließlich fehlt noch der blaue Unterarm.

> **TODO**
>
> - Fügt nach dem selben Schema wie oben den blauen Unterarm als Kindobjekt des grünen Arms ein.
>   Dabei muss der blaue Arm auch aus zwei Hierarchie-Ebenen bestehen: Einer, die den Nullpunkt
>   in das Scharnier zwischen Grün und Blau setzt und einer Kind-Ebene, die die Geometrie mit ihrem
>   Nullpunkt in der Mitte an die richtige Stelle setzt.
>
> - Macht u.U. Skizzen: Zeichnet den Szenengraphen auf und malt die Ursprünge der lokalen Koordinatensysteme
>   in o.A. Zeichnung ein. Setzt alle Drehwinkel auf 0 zurück, um den Roboter in der nach oben gestreckten
>   Grundhaltung aufzubauen.
>
> - Sollte es gar nicht klappen, verwendet ***AUSNAHMSWEISE*** als Vorgriff die Implementierung im
>   [_Completed_-Projekt](../09_HierarchyAndInputCompleted/Core/HierarchyInput.cs#L27)

Als Ergebnis sollte der Roboter mit allen Armen so konstruiert sein, dass Drehung der roten Säule um die
Y-Achse und Drehungen der beiden Arme (grün, blau) jeweils um die X-Achse den Roboter in beliebige Stellunge
bringen, ohen die Einzelteile auseinander zu reißen.

![Cuboter Fertig](_images/RoboCompleted.png)

## Eingabe










