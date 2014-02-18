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

        int c = 0;
        c = a + b;
        return c;
    }
    

    
    public override double Tehtava2(double x, double y, double z)
    {
        // Tehtävänanto: Palauta muuttujien z, y ja z keskiarvo
        // http://tilastokoulu.stat.fi/verkkokoulu_v2.xql?page_type=sisalto&course_id=tkoulu_tlkt&lesson_id=4&subject_id=4

        return (x + y + z) / 3;
    }
    

    
    public override void Tehtava3()
    {
        // Tehtävänanto: Lisää peliin punainen pallo 

        pallo = new PhysicsObject(40,40);
        pallo.Shape = Shape.Circle;
        pallo.Color = Color.Red;
        Add(pallo);
    }
    

    
    public override void Tehtava4(int n)
    {
        // Tehtävänanto: Lisää peliin n kpl satunnaisessa paikassa olevaa VALKOISTA palloa
        //   (vinkki, käytä for-silmukkaa)

        // Tässä tehdään satunnainen paikka korkeintaan 300.0 päähän ruudun keskipisteestä
        for (int i = 0; i < n; i++)
        {
            Vector paikka = RandomGen.NextVector(0.0, 300.0);
            valkoisetPallot = new PhysicsObject(40, 40);
            valkoisetPallot.Shape = Shape.Circle;
            valkoisetPallot.Position = paikka;
            //valkoisetPallot.Color = Color.Green;
            valkoisetPallot.Tag = "pallo";
            Add(valkoisetPallot);

            //AddCollisionHandler(valkoisetPallot, "pallo", PallotTormasivat);
        }
    }
    

    
    public override void Tehtava5()
    {
        // Tehtävänanto: Lisää peliin reunat joka puolelle

        //Level.CreateBorders();
        Level.CreateTopBorder();
        Level.CreateBottomBorder();
        Level.CreateLeftBorder();
        Level.CreateRightBorder(1.0, false);
        //Camera.ZoomToLevel();
        //Camera.Zoom(0.5);
        //PhysicsObject pallo = new PhysicsObject(100, 100);
        //pallo.Shape = Shape.Circle;
        //Add(pallo);

        // TODO: Ota tehtävä pois kommenteista ja toteuta
    }



    public override void Tehtava6()
    {
        // Tehtävänanto: Lyö PUNAISELLE pallolle vauhtia satunnaiseen suuntaan
        //  (vinkki, tee Tehtava3-aliohjelmassa lisättävästä pallosta luokkamuuttuja)

        // TODO: Ota tehtävä pois kommenteista ja toteuta

        Vector isku = RandomGen.NextVector(0.0, 700.0);
        pallo.Hit(isku);
    }
    

    
    public override void Tehtava7(List<GameObject> pallot)
    {
        // Tehtävänanto: Kun kaksi VALKOISTA palloa osuu toisiinsa, pistä ne katoamaan
        //  lisäpisteitä jos saat ne räjähtämään (kersku siitä kaverille ja opettajille :)
        //  (vinkki, käy pallot-lista läpi for-silmukassa ja lisää törmäyskäsittelijä,
        //  tarvitset myös uuden aliohjelman)

        for (int i = 0; i < pallot.Count; i++)
        {
            AddCollisionHandler(pallot[i] as PhysicsObject, PallotTormasivat);
        }

        //MessageDisplay.Add("Tehtävä 7");

        // TODO: Ota tehtävä pois kommenteista ja toteuta

    }



    public override void Tehtava8()
    {
        // Tehtävänanto: Aina kun välilyöntiä painetaan, lyö PUNAISELLE pallolle lisää vauhtia.
        // (vinkki: uudelleenkäytä Tehtava6-aliohjelmaa kutsumalla sitä näin "Tehtava6")

        Keyboard.Listen(Key.Space, ButtonState.Pressed, Tehtava6, "iske palloa");
        
        // TODO: Ota tehtävä pois kommenteista ja toteuta
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
