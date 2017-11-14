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
egy 20x20-as hálózatot húzunk ki, ami valójában 22x22, ennek az az értelme, hogy az első és az utolsó sor (és oszlop) valójában a fal, ha mozgatjuk a kígyót, akkor nem kell azzal törődnünk, hogy mozoghat-e, mert egyet még tudunk lépni a látható tábláról: ekkor a falba mozogtunk, de ehhez nem kell esetszétválasztás.

A megjelenítéskor a [FontAwesome ikonkészletet](http://fontawesome.io/icons/) használjuk, ehhez a megfelelő nuget csomagot telepítjük ([FontAwesome.WPF](https://www.nuget.org/packages/FontAwesome.WPF/))

A telepítés három dolgot végez el:
- beírja magát a packages.config könyvtárba
- letölti a csomagot a Solution mappa Packages könyvtárjába
- létrehoz egy hivatkozást a projektünkbe ami a megfelelő dll-t a packages mappából meghivatkozza.

a középső 20x20-as részt feltöltjük ikonokkal

ahhoz, hogy az ikonokat az XAML-ben használni tudjuk, kell a névtér hivatkozás a definícióba. XAML névtérhovatkozás kell: ami valójában XML névtér hivatkozás: xmlns, vagyis xml name space.

Ezt legegyszerűbb lelopni a nuget csomagnak a doksijából ([a githubról](https://github.com/charri/Font-Awesome-WPF/blob/master/README-WPF.md))


## Tennivalók (2017.11.14)
- Hibajavítás, 
  - az utolsó sorba és oszlopba nem teszünk semmit, 
  - illetve, az utolsó sorba és az utolsó oszlopba nem tudom a kígyót kormányozni


