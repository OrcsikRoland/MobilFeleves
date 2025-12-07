# HikeMate – túra- és edzésnapló (ötletterv)

A féléves projekt egy .NET MAUI alkalmazás, amely támogatja a felhasználót túrák és szabadtéri
edzések tervezésében, rögzítésében és megosztásában. A cél egy MVVM alapú, stabil alkalmazás,
amely teljesíti a féléves követelményeket: több navigálható oldal, helyi adatbázisra épülő CRUD
műveletek és legalább két szenzoros funkció hasznos integrálása.

## Fő funkciók
- **Túrák listája és részletei**: helyi SQLite adatbázisban tárolt túra bejegyzések (név, leírás,
táv, szintemelkedés, dátum, időtartam, fotó útvonaldíszítéshez, aktuális időjárás-összegzés). 
- **CRUD műveletek**: új túra felvétele, meglévő módosítása, törlése és részleteinek megtekintése,
a felhasználó által megadott értékekkel (nem generált vagy önkényesen módosított adatok).
- **Kamera**: túra bejegyzéshez fotó készítése és mentése, amelyet a felhasználó a részleteknél
lát és megoszthat.
- **Megosztás**: összefoglaló megosztása szöveges formában (név, táv, szint, idő, link a fotóhoz),
például üzenetküldőn keresztül.

## Oldalstruktúra (legalább 3 navigálható Page)
1. **DashboardPage**: kiemelt következő túra, gyors műveletek (új túra, legutóbbi megnyitása).
2. **TripListPage**: összes túra listája kereséssel/szűréssel; innen nyílik a részlet oldal, illetve
lehet új bejegyzést létrehozni vagy szerkeszteni.
3. **TripDetailPage**: a kiválasztott túra részletei, a túrához készített fotó megjelenítése,
   CRUD műveletek (szerkesztés, törlés), megosztás gomb és kamera indítása;
   innen érhető el **TripEditPage** (nem statikus, validált űrlap az adatok bevitelére).

> A statikus jellegű oldalak (pl. „About”) nem számítanak bele a minimumba; a fenti
oldalak mind interaktívak és a felhasználó által szerkeszthető adatokat tartalmaznak.

## Adatmodell és technológia
- **Entitás**: `Trip` (azonosító, név, leírás, táv, szintemelkedés, dátum, időtartam, kezdő koordináta,
  fotó elérési út, opcionális időjárás-összegzés, szinkron státusz).
- **Adatbázis**: SQLite lokális tárolás `TripRepository`-val; CRUD az MVVM nézetmodelleken
  keresztül, `ICommand`-okkal és adatvalidációval.
- **Szenzorok és szolgáltatások**: `MediaPicker`/kamera a fotókhoz és `Share` API a szöveges
  összefoglalóhoz.
- **MVVM**: `BaseViewModel` értesítési mechanizmussal, külön viewmodel a dashboard,
lista, részlet és szerkesztés oldalakhoz; `Dependency Injection` a szolgáltatásokhoz.

## Minimumkövetelmények lefedése
- MVVM szerkezet: viewmodel réteg, DI, parancsok, validáció.
- 3+ interaktív Page: Dashboard, Trip list, Trip detail (plusz szerkesztő modal/oldal).
- CRUD: `Trip` entitás teljes körűen, felhasználói bevitelre támaszkodva.
- Két szenzoros funkció: kamera és megosztás (mindkettő érdemi funkcióként bekötve).
- Stabilitás: input-validáció, hibakezelés a CRUD-hoz.

## Javasolt mérföldkövek
1. **Projektszerkezet és MVVM alap**: viewmodel-ek, DI, navigációs shell.
2. **Adatbázis és CRUD**: SQLite repository, Trip űrlap validációval.
3. **Kamera + megosztás**: fotómentés, összefoglaló megosztása.
4. **Finomhangolás**: hibakezelés, UI polírozás, teljesítményjavítás.

A fenti terv a laborvezető jóváhagyására alkalmas alapot ad, és követi a féléves feladat
követelményeit, egy jól körülhatárolt, hasznos alkalmazásötlettel.
