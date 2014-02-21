using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;
using System.Runtime.CompilerServices;


class Harjoitukset : Tarkistaja
{
    /* POISTA TÄMÄ RIVI...
    public override int Tehtava1(int a, int b)
    {
        // Tehtävänanto: Palauta muuttujien a ja b summa (plus lasku)

        int c = ?;
        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
        return c;
    }
    ...JA POISTA TÄMÄ RIVI */


    /* POISTA TÄMÄ RIVI...
    public override double Tehtava2(double x, double y, double z)
    {
        // Tehtävänanto: Palauta muuttujien z, y ja z keskiarvo
        //
        // Vinkki: http://bit.ly/OieLwT
        //  huomaa myös, että a+b/2 antaa eri tuloksen kuin (a+b)/2 

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
        return ?;
    }
    ...JA POISTA TÄMÄ RIVI */


    /* POISTA TÄMÄ RIVI...
    public override void Tehtava3()
    {
        // Tehtävänanto: Lisää peliin fysiikan lakeja noudattava punainen pallo 

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
    }
    ...JA POISTA TÄMÄ RIVI */


    /* POISTA TÄMÄ RIVI...
    public override void Tehtava4(int n)
    {
        // Tehtävänanto: Lisää peliin n kpl satunnaisessa paikassa olevaa VALKOISTA palloa
        //   jotka noudattavat fysiikan lakeja
        //
        // Vinkki: käytä for-silmukkaa, lisäksi alla on annettuna koodi, jolla arvotaan
        //  satunnainen paikka korkeintaan 300.0 yksikön päähän ruudun keskipisteestä.
        
        // Vector paikka = RandomGen.NextVector(0.0, 300.0);

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
    }
    ...JA POISTA TÄMÄ RIVI */


    /* POISTA TÄMÄ RIVI...
    public override void Tehtava5()
    {
        // Tehtävänanto: Lisää peliin reunat joka puolelle

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
    }
    ...JA POISTA TÄMÄ RIVI */


    /* POISTA TÄMÄ RIVI...
    public override void Tehtava6()
    {
        // Tehtävänanto: Lyö PUNAISELLE pallolle vauhtia satunnaiseen suuntaan
        //
        // Vinkki, tee Tehtava3-aliohjelmassa lisättävästä pallosta luokkamuuttuja.

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
    }
    ...JA POISTA TÄMÄ RIVI */
    

    /* POISTA TÄMÄ RIVI...
    public override void Tehtava7(List<PhysicsObject> pallot)
    {
        // Tehtävänanto: Kun kaksi VALKOISTA palloa osuu toisiinsa, pistä ne katoamaan
        //  lisäpisteitä jos saat ne räjähtämään (kersku siitä kaverille ja opettajille :)
        //
        // Vinkki: käy pallot-lista läpi for-silmukassa ja lisää törmäyskäsittelijä,
        //  ja sille uusi aliohjelma.

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
    }
    ...JA POISTA TÄMÄ RIVI */


    /* POISTA TÄMÄ RIVI...
    public override void Tehtava8()
    {
        // Tehtävänanto: Aina kun välilyöntiä painetaan, lyö PUNAISELLE pallolle lisää vauhtia.
        // 
        // Vinkki: uudelleenkäytä Tehtava6-aliohjelmaa tapahtumakäsittelijässä.

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
    }
    ...JA POISTA TÄMÄ RIVI */


    /* POISTA TÄMÄ RIVI...
    public override void Tehtava9(int pallojaTallaHetkella)
    {
        // Tehtävänanto: Lisää ruudulla näkyvä laskuri, joka pitää kirjaa siitä montako VALKOISTA
        //  palloa on vielä pelissä.
        //
        // Vinkki: Tee laskurille luokkamuuttuja, jonka arvo on aluksi null (eli ei arvoa).
        //  Tässä aliohjelmassa luo sitten se laskuri ja näyttö. Uudelleenkäytä Tehtävän 7
        //  törmäyskäsittelijää, jossa muuta laskurin arvoa VAIN JOS SE EI OLE null.

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
    }
    ...JA POISTA TÄMÄ RIVI */


    /* POISTA TÄMÄ RIVI...
    public override void Tehtava10()
    {
        // Tehtävänanto: Kun jäljellä on enää alle puolet VALKOISISTA palloista, vaihda kentän 
        //  taustaväri vaaleanpunaiseksi (Color.Pink).
        //
        // Vinkki: Kutsu tätä aliohjelmaa tehtävän 7 törmäyskäsittelijässä. Kannattaa myös käyttää 
        //  apuna tehtävän 9 laskuria.

        // TODO: Ota tehtävä pois kommenteista ja kirjoita toteutus tähän
    }
    ...JA POISTA TÄMÄ RIVI */
}
