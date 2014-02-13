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
    PhysicsObject pallo = null;
    PhysicsObject valkoisetPallot;
    
    public override int Tehtava1(int a, int b)
    {
        // Tehtävänanto: Palauta muuttujien a ja b summa (plus lasku)

        int c = 0; // TODO: Ota tehtävä pois kommenteista ja toteuta
        c = a + b;
        return c;
    }
    

    
    public override double Tehtava2(double x, double y, double z)
    {
        // Tehtävänanto: Palauta muuttujien z, y ja z keskiarvo
        // http://tilastokoulu.stat.fi/verkkokoulu_v2.xql?page_type=sisalto&course_id=tkoulu_tlkt&lesson_id=4&subject_id=4

        // TODO: Ota tehtävä pois kommenteista ja toteuta

        return (x + y + z) / 3;
    }
    

    
    public override void Tehtava3()
    {
        // Tehtävänanto: Lisää peliin pallo 

        // TODO: Ota tehtävä pois kommenteista ja toteuta

        PhysicsObject pallo = new PhysicsObject(40,40);
        pallo.Shape = Shape.Circle;
        //pallo.Color = Color.Red;
        Add(pallo);
    }
    

    
    public override void Tehtava4()
    {
        // Tehtävänanto: Lisää PUNAINEN pallo peliin satunnaiseen paikkaan

        // Tässä tehdään satunnainen paikka korkeintaan 100.0 päähän ruudun keskipisteestä
        Vector paikka = RandomGen.NextVector(0.0, 100.0);

        pallo = new PhysicsObject(40, 40);
        pallo.Shape = Shape.Circle;
        pallo.Color = Color.Red;
        pallo.Position = paikka;
        //pallo.Position = new Vector(200, 200);
        Add(pallo);

        // TODO: Ota tehtävä pois kommenteista ja toteuta
    }
    

    
    public override void Tehtava5(int n)
    {
        // Tehtävänanto: Lisää peliin n kpl satunnaisessa paikassa olevaa VALKOISTA palloa
        //   (vinkki, käytä for-silmukkaa)

        for (int i = 0; i < n; i++ )
        {
            Vector paikka = RandomGen.NextVector(0.0, 500.0);
            valkoisetPallot = new PhysicsObject(40, 40);
            valkoisetPallot.Shape = Shape.Circle;
            valkoisetPallot.Position = paikka;
            valkoisetPallot.Tag = "pallo";
            Add(valkoisetPallot);

            AddCollisionHandler(valkoisetPallot, "pallo", PallotTormasivat);
        }

        // TODO: Ota tehtävä pois kommenteista ja toteuta
    }



    public override void Tehtava6()
    {
        // Tehtävänanto: Lisää peliin reunat joka puolelle

        // TODO: Ota tehtävä pois kommenteista ja toteuta

        
        
    }
    

    
    public override void Tehtava7()
    {
        // Tehtävänanto: Lyö PUNAISELLE pallolle vauhtia satunnaiseen suuntaan
        //  (vinkki, tee Tehtava4-aliohjelmassa lisättävästä pallosta luokkamuuttuja)

        // TODO: Ota tehtävä pois kommenteista ja toteuta

        Vector isku = RandomGen.NextVector(0.0, 300.0);
        pallo.Hit(isku);
    }



    public override void Tehtava8()
    {
        // Tehtävänanto: Kun kaksi VALKOISTA palloa osuu toisiinsa, pistä ne katoamaan
        //  lisäpisteitä jos saat ne räjähtämään (kersku siitä kaverille ja opettajille :)
        //  (vinkki, tee uudet valkoiset pallot kutsumalla Tehtava5 aliohjelmaa,
        //  tarvitset törmäyskäsittelijän ja myös uuden aliohjelman)

        Tehtava5(150);
        AddCollisionHandler(valkoisetPallot, PallotTormasivat);

        MessageDisplay.Add("Tehtävä 8");

        // TODO: Ota tehtävä pois kommenteista ja toteuta

        // ei virheitä, palauta 1
        //return 1;
    }
    


    public override void Tehtava9()
    {
        // Tehtävänanto: Aina kun välilyöntiä painetaan, lyö PUNAISELLE pallolle lisää vauhtia.
        // (vinkki: uudelleenkäytä Tehtava7-aliohjelmaa kutsumalla sitä näin "Tehtava7();")

        // TODO: Ota tehtävä pois kommenteista ja toteuta
        
        // ei virheitä, palauta 1
        //return 0; 
    }



    public override void Tehtava10()
    {
        // Tehtävänanto: Lisää ruudulle laskuri, joka pitää kirjaa siitä montako VALKOISTA
        //  palloa on vielä pelissä. Kun kaikki valkoiset pallot ovat kadonneet, lisää uusi
        //  satsi palloja käyttäen Tehtava5()-aliohjelmaa.

        // TODO: Ota tehtävä pois kommenteista ja toteuta
        
        // ei virheitä, palauta 1
        //return 0; 
    }


    void PallotTormasivat(PhysicsObject pallo, PhysicsObject pallo2)
    {
        //MessageDisplay.Add("Valkoiset pallot törmäsivät");
        if (pallo.Tag == pallo2.Tag)
        {
            pallo.Destroy();
            pallo2.Destroy();
        }
    }
}
