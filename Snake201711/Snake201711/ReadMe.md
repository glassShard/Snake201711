# Snake projekt

## Rövid leírás
A Snake játék lényege, hogy van egy játéktér, ami téglalap alakú, erről nem lehet lemenni. Van rajta egy kígyó
aminek a fejében ülünk és a feje irányításával mozgatjuk a kígyót. A cél: a játéktéren felbukkanó ételeket megegenni.
minden evés alkalmával a kígyónk hossza megnő, és minél tovább játszunk, annál hosszabb a kígyónk. Így a feladat 
egyre nehezebb: elkerülni a saját kígyónkat, és nem beleütközni a falba.

```
+----------------------------------------------------------------------------------+
|                                                                                  |
|                                                                                  |
|                                                                                  |
|          a                                                                       |
|                                                            s                     |
|                                                                                  |
|                                                                                  |
|                                                                                  |
|                                                                                  |
|                                                                                  |
|            +--------->                       a                                   |
|            |                                                                     |
|            |                                                                     |
|            +                                                                     |
|                                                                                  |
|                                                                a                 |
|                                                                                  |
|                              v                                                   |
|                                                                                  |
|                                                                                  |
|                                                                                  |
+----------------------------------------------------------------------------------+
```
## A játékunk kinézete

```
                                                                X  <--------+ Kilépés
+-----------------------------------------+-------------------+
|                                         |                   |
|                                         | Pontszám: 3120    |
|                                         |                   |
|                                         | Megevett étel: 10 |
|                                         |                   |
|                                         | Kígyó hossza: 5   |
|                                         |                   |
|                                         | Játékidő: 03:56   |
|                                         |                   |
|                                         |                   |
|                                         |                   |
|                                         |                   |
|                                         |                   |
|                                         |                   |
|                                         |                   |
|                                         |                   |
|                                         |                   |
|                                         |                   |
|                                         |  +-------+------+ |
|                                         |  | Start | Stop | |
|                                         |  +-------+------+ |
+-----------------------------------------+-------------------+
```

## Játéktér kialakítása XAML-ben
egy 20x20-as hálózatot húzunk ki