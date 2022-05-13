var nadawca = new Klient();
var odbiorca = new Klient();

var paczkomat = nadawca.Wyslij("paczkomat", odbiorca);
odbiorca.OdbierzWPaczkomacie((Paczkomat) paczkomat);

var kurier = nadawca.Wyslij("kurier", odbiorca);