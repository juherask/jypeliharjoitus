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
    IntMeter laskuri = null;
    int pallojenLkm = 100;
    
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
        pallojenLkm = n;

        for (int i = 0; i < n; i++)
        {
            Vector paikka = RandomGen.NextVector(0.0, 300.0);
            PhysicsObject valkoisetPallot = new PhysicsObject(40, 40);
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

        Level.CreateBorders();
        /*Level.CreateTopBorder();
        Level.CreateBottomBorder();
        Level.CreateLeftBorder();
        Level.CreateRightBorder(1.0, false);*/
        Camera.ZoomToLevel();
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
    
    public override void Tehtava9(int pallojaTallaHetkella)
    {
        // Tehtävänanto: Lisää ruudulla näkyvä laskuri, joka pitää kirjaa siitä montako VALKOISTA
        //  palloa on vielä pelissä.
        // Vinkki: Tee laskurista luokkamuuttuja, jonka arvo on aluksi null (eli ei arvoa).
        //  sen jälkeen uudelleenkäytä Tehtävän 7 törmäyskäsittelijää, jossa muuta 
        //  laskurin arvoa VAIN JOS SE EI OLE null.

        laskuri = new IntMeter(pallojaTallaHetkella);
        Label naytto = new Label();
        naytto.BindTo(laskuri);

        Add(naytto);

        // TODO: Ota tehtävä pois kommenteista ja toteuta
    }



    public override void Tehtava10()
    {
        // Tehtävänanto: Kun jäljellä on enää alle puolet VALKOISISTA palloista, vaihda kentän 
        //  taustaväri vaaleanpunaiseksi (Color.Pink).
        // Vinkki: Kutsu tätä aliohjelmaa tehtävän 7 törmäyskäsittelijässä. Kannattaa myös käytää 
        //  apuna tehtävän 9 laskuria.
        

        // TODO: Ota tehtävä pois kommenteista ja toteuta
    }


    void PallotTormasivat(PhysicsObject pallo, PhysicsObject pallo2)
    {
        //MessageDisplay.Add("Valkoiset pallot törmäsivät");
        if (pallo.Tag == pallo2.Tag)
        {
            pallo.Destroy();
            pallo2.Destroy();

            if (laskuri != null)
            {
                laskuri.Value = laskuri.Value - 1;

                if (laskuri.Value < pallojenLkm * 0.5)
                    Level.Background.Color = Color.Pink;
            }
        }
    }
}
