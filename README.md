# HikeMate – túra- és edzésnapló (ötletterv)

A féléves projekt egy .NET MAUI alkalmazás, amely támogatja a felhasználót túrák és szabadtéri
edzések tervezésében, rögzítésében és megosztásában. A cél egy MVVM alapú, stabil alkalmazás,
amelyi teljesíti a féléves követelményeket: több navigálható oldal, helyi adatbázisra épülő CRUD
műveletek és legalább két szenzor/hálózati funkció hasznos integrálása.

## Fő funkciók
- **Túrák listája és részletei**: helyi SQLite adatbázisban tárolt túra bejegyzések (név, leírás,
táv, szintemelkedés, dátum, időtartam, fotó útvonaldíszítéshez, aktuális időjárás-összegzés). 
- **CRUD műveletek**: új túra felvétele, meglévő módosítása, törlése és részleteinek megtekintése,
a felhasználó által megadott értékekkel (nem generált vagy önkényesen módosított adatok).
- **Geolokáció és térkép**: a kezdőpozíció automatikus javaslata GPS-ből, térképes megjelenítés
az indulási pontról és a rögzített útvonalról.
- **Kamera**: túra bejegyzéshez fotó készítése és mentése, amelyet a felhasználó a részleteknél
lát és megoszthat.
- **Megosztás**: összefoglaló megosztása szöveges formában (név, táv, szint, idő, link a fotóhoz),
például üzenetküldőn keresztül.
- **Hálózatkezelés**: az alkalmazás jelzi, ha nincs internet, és offline módban is engedi a CRUD
műveleteket; online állapotban időjárás-összegzést tölt le az indulási ponthoz.

## Oldalstruktúra (legalább 3 navigálható Page)
1. **DashboardPage**: kiemelt következő túra, gyors műveletek (új túra, legutóbbi megnyitása),
hálózati állapot és utolsó időjárás-frissítés jelzése.
2. **TripListPage**: összes túra listája kereséssel/szűréssel; innen nyílik a részlet oldal, illetve
lehet új bejegyzést létrehozni vagy szerkeszteni.
3. **TripDetailPage**: a kiválasztott túra részletei, térképes nézet a kezdőpontról/útvonalról,
CRUD műveletek (szerkesztés, törlés) és megosztás gomb; innen érhető el **TripEditPage**
(nem statikus, validált űrlap az adatok bevitelére és kamera-használatra).

> A statikus jellegű oldalak (pl. „About”) nem számítanak bele a minimumba; a fenti
oldalak mind interaktívak és a felhasználó által szerkeszthető adatokat tartalmaznak.

## Adatmodell és technológia
- **Entitás**: `Trip` (azonosító, név, leírás, táv, szintemelkedés, dátum, időtartam, kezdő GPS
koordináta, fotó elérési út, opcionális időjárás-összegzés, szinkron státusz).
- **Adatbázis**: SQLite lokális tárolás `TripRepository`-val; CRUD az MVVM nézetmodelleken
keresztül, `ICommand`-okkal és adatvalidációval.
- **Szenzorok és szolgáltatások**: `IGeolocation` a kezdőpont és útvonal méréshez,
`MediaPicker`/kamera a fotókhoz, `Connectivity` a hálózati állapothoz, `Map` vezérlő a
vizualizációhoz, `Share` API a szöveges összefoglalóhoz.
- **MVVM**: `BaseViewModel` értesítési mechanizmussal, külön viewmodel a dashboard,
lista, részlet és szerkesztés oldalakhoz; `Dependency Injection` a szolgáltatásokhoz.

## Minimumkövetelmények lefedése
- MVVM szerkezet: viewmodel réteg, DI, parancsok, validáció.
- 3+ interaktív Page: Dashboard, Trip list, Trip detail (plusz szerkesztő modal/oldal).
- CRUD: `Trip` entitás teljes körűen, felhasználói bevitelre támaszkodva.
- Két szenzor/hálózati funkció: GPS+térkép, kamera, megosztás, hálózatkezelés (legalább kettő
közülük implementálva érdemi funkcióként).
- Stabilitás: input-validáció, hibakezelés, offline-first működés a CRUD-hoz.

## Javasolt mérföldkövek
1. **Projektszerkezet és MVVM alap**: viewmodel-ek, DI, navigációs shell.
2. **Adatbázis és CRUD**: SQLite repository, Trip űrlap validációval.
3. **Geolokáció + térkép**: kezdőpont felvétele, térképes megjelenítés a részlet oldalon.
4. **Kamera + megosztás**: fotómentés, összefoglaló megosztása.
5. **Hálózatkezelés + finomhangolás**: offline/online állapot kezelése, hibatűrés, UI polírozás.

A fenti terv a laborvezető jóváhagyására alkalmas alapot ad, és követi a féléves feladat
követelményeit, egy jól körülhatárolt, hasznos alkalmazásötlettel.
