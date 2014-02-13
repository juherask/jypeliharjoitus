using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;
using System.Linq;

public abstract class Tarkistaja : PhysicsGame
{
    enum TehtavanTila
    {
        EiToteutettu,
        ToteutettuSaattaaToimia,
        ToteutettuEiToimi,
        ToteutettuToimii,
    }

    int PACPADDING = 60;
    int PACSPEED = 333;
    int TEHTAVIA = 10;
    int tehtava = 0;
    GameObject pacman;
    GameObject haamut;
    List<GameObject> kolikot;

    // Tehtävien tilatiedot
    List<GameObject> objektitEnnenTehtavaa3 = null;

    public override void Begin()
    {
        SetWindowSize(1280, 720);

        Animation pacanim = new Animation( LoadImages("pac1","pac2") );
        pacman = new GameObject(pacanim);
        pacman.X = Screen.Left + PACPADDING;
        pacman.Y = Screen.Bottom + PACPADDING;
        Add(pacman, 2);
        pacman.Animation.FPS = 3;
        pacman.Animation.Start();

        Animation haamuanim = new Animation(LoadImages("haamu1", "haamu2", "haamu3"));
        haamut = new GameObject(haamuanim);
        haamut.X = Screen.Right + PACPADDING + haamut.Width / 2;
        haamut.Y = Screen.Bottom + PACPADDING;
        Add(haamut, 2);
        haamut.Animation.FPS = 3;
        haamut.Animation.Start();

        kolikot = new List<GameObject>();
        double kolikkoy = pacman.Y;
        double kolikkox = pacman.X;
        for (int i = 0; i < TEHTAVIA; i++)
        {
            kolikkox += (Screen.Right - PACPADDING - pacman.X) / TEHTAVIA;

            GameObject kolikko = new GameObject(40,40);
            kolikko.Image = LoadImage("coin");
            kolikko.X = kolikkox;
            kolikko.Y = kolikkoy;
            kolikot.Add(kolikko);
            Add(kolikko, 1);
        }
        
        Timer.SingleShot(0.33, TarkistaTehtava);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    private double PoistaKolikko()
    {
        double seuraavaanTarkastukseen = 1.00;
        if (tehtava > 0)
        {
            if (tehtava > 1)
                kolikot[tehtava - 2].Destroy();
            Vector coinPos = kolikot[tehtava - 1].Position;
            coinPos.X += 20;
            pacman.MoveTo(coinPos, PACSPEED);

            // Prevent removing coin before check
            seuraavaanTarkastukseen = Math.Abs(pacman.X - coinPos.X) / PACSPEED;
        }
        return seuraavaanTarkastukseen;
    }

    private void TarkistaTehtava()
    {
        TehtavanTila tulos = TehtavanTila.EiToteutettu;

        switch (tehtava)
        {
            case 1:
                tulos = TarkistaTehtava1();
                break;
            case 2:
                tulos = TarkistaTehtava2();
                break;
            case 3:
                tulos = TarkistaTehtava3();
                break;
            case 4:
                //TODO: Aki/Jussi kirjoita tarkistakoodi
                break;
            case 5:
                //TODO: Aki/Jussi kirjoita tarkistakoodi
                break;
            case 6:
                //TODO: Aki/Jussi kirjoita tarkistakoodi
                break;
            case 7:
                //TODO: Aki/Jussi kirjoita tarkistakoodi
                break;
            case 8:
                //TODO: Aki/Jussi kirjoita tarkistakoodi
                break;
            case 9:
                //TODO: Aki/Jussi kirjoita tarkistakoodi
                break;
            case 10:
                //TODO: Aki/Jussi kirjoita tarkistakoodi
                break;
            default:
                tulos = TehtavanTila.ToteutettuToimii;
                break;
        }

        // Toimi tuloksen mukaisesti
        if (tulos == TehtavanTila.ToteutettuToimii)
        {
            double seuraavaanTarkastukseen = PoistaKolikko();
            tehtava++;

            // Try as long as it takes.
            Timer.SingleShot(seuraavaanTarkastukseen, TarkistaTehtava);
        }
        else if (tulos == TehtavanTila.ToteutettuEiToimi)
        {
            PoistaKolikko();
            Vector ulosVasemmalta = new Vector(Screen.Left-haamut.Width/2, pacman.Y);
            Animation peruutusAnimaatio = Animation.Mirror(new Animation(LoadImages("pac1", "pac2")));
            pacman.Animation = peruutusAnimaatio;
            pacman.Animation.FPS = 5;
            pacman.Animation.Start();
            pacman.MoveTo(ulosVasemmalta, PACSPEED);
            haamut.MoveTo(ulosVasemmalta, PACSPEED);
        }
        else if (tulos == TehtavanTila.ToteutettuSaattaaToimia)
        {
            Timer.SingleShot(0.01, TarkistaTehtava);
        }
        
    }

    private TehtavanTila TarkistaTehtava3()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            if (objektitEnnenTehtavaa3 == null)
            {
                // Precondition
                objektitEnnenTehtavaa3 = GetObjects(go => go is PhysicsObject);
                Tehtava3();
                // Tehtavan kutsumisen jälkeen pitää antaa JyPelille aikaa tehdä työnsä.
                //  tähän tarkistajaan tullaan siis myöhemmin uudelleen, mutta tätä haaraa ei enää tehdä.
                tila = TehtavanTila.ToteutettuSaattaaToimia;
                return tila;
            }

            List<GameObject> uudet = GetObjects(go => go is PhysicsObject && !objektitEnnenTehtavaa3.Contains(go));

            tila = TehtavanTila.ToteutettuEiToimi;
            if (uudet.Count == 0)
            {
                MessageDisplay.Add("Palloa edustavaa pelioliota ei ole lisätty");
            }
            else if (uudet.Count > 1)
            {
                MessageDisplay.Add("Liian monta uutta pelioliota lisätty");
            }
            else if (uudet[0].Shape != Shape.Circle)
            {
                MessageDisplay.Add("Lisäämäsi pallo ei ole pyöreä");
            }
            else if (uudet[0].Color != Color.White)
            {
                MessageDisplay.Add("Olet vaihtanut pallon väriä. Kiva, mutta lisäämäsi pallon tulisi olla valkoinen");
            }
            else
            {
                tila = TehtavanTila.ToteutettuToimii;
            }
        }
        catch (NotImplementedException)
        {
            tila = TehtavanTila.EiToteutettu;
        }
        return tila;
    }

    private TehtavanTila TarkistaTehtava2()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            tila = TehtavanTila.ToteutettuToimii;

            double TOLERANCE = 0.01;
            for (int i = 0; i < 100; i++)
            {
                double x = RandomGen.NextDouble(-10.0, 10.0);
                double y = RandomGen.NextDouble(-10.0, 10.0);
                double z = RandomGen.NextDouble(-10.0, 10.0);
                double ka = (x + y + z) / 3;
                if (Math.Abs(ka - Tehtava2(x, y, z)) > TOLERANCE)
                {
                    string virhe = String.Format(@"Virhe: Koodi ei laske oikein keskiarvoa. Esim. koodi antaa
{0}, {1}, {2} keskiarvoksi {3} kun sen pitäisi olla {4}", x, y, z, Tehtava2(x, y, z), (x + y + z / 3));
                    MessageDisplay.Add(virhe);

                    tila = TehtavanTila.ToteutettuEiToimi;
                    break;
                }
            }
        }
        catch (NotImplementedException)
        {
            tila = TehtavanTila.EiToteutettu;
        }
        return tila;
    }

    private TehtavanTila TarkistaTehtava1()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            tila = TehtavanTila.ToteutettuToimii;

            for (int i = 0; i < 100; i++)
            {
                int a = RandomGen.NextInt(10);
                int b = RandomGen.NextInt(10);
                if (a + b != Tehtava1(a, b))
                {
                    string virhe = String.Format("Virhe: Koodi ei laske oikein lukuja yhteen. Esim. {0}+{1}={2}, kun pitäisi olla {3}", a, b, Tehtava1(a, b), a + b);
                    MessageDisplay.Add(virhe);
                    tila = TehtavanTila.ToteutettuEiToimi;
                    break;
                }   
            }
        }
        catch (NotImplementedException)
        {
            tila = TehtavanTila.EiToteutettu;
        }
        return tila;
    }


    /*
     * Tehtävänanto: Palauta muuttujien a ja b summa (plus lasku)
     */
    public virtual int Tehtava1(int a, int b) { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Palauta muuttujien z, y ja z keskiarvo
     * http://tilastokoulu.stat.fi/verkkokoulu_v2.xql?page_type=sisalto&course_id=tkoulu_tlkt&lesson_id=4&subject_id=4
     */
    public virtual double Tehtava2(double x, double y, double z) { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Lisää peliin pallo 
     */
    public virtual void Tehtava3() { throw new NotImplementedException(); }
    

    /*
     * Tehtävänanto: Lisää PUNAINEN pallo peliin satunnaiseen paikkaan
     * Tässä tehdään satunnainen paikka korkeintaan 100.0 päähän ruudun keskipisteestä
     *   Vector paikka = RandomGen.NextVector(0.0, 100.0);
     */
    public virtual void Tehtava4() { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Lisää peliin n kpl satunnaisessa paikassa olevaa VALKOISTA palloa
     * (vinkki, käytä for-silmukkaa)
     */
    public virtual void Tehtava5(int n) { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Lisää peliin reunat joka puolelle
     */
    public virtual void Tehtava6() { throw new NotImplementedException(); }
    
    /*
     * Tehtävänanto: Lyö PUNAISELLE pallolle vauhtia satunnaiseen suuntaan
     *  (vinkki, tee Tehtava4-aliohjelmassa lisättävästä pallosta luokkamuuttuja) 
     */
    public virtual void Tehtava7() { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Kun kaksi VALKOISTA palloa osuu toisiinsa, pistä ne katoamaan
     *  lisäpisteitä jos saat ne räjähtämään (kersku siitä kaverille ja opettajille :)
     *  (vinkki, tarvitset törmäyskäsittelijää ja uuden aliohjelman)
     */
    public virtual void Tehtava8() { throw new NotImplementedException(); }
        

    /*
     * Tehtävänanto: Aina kun välilyöntiä painetaan, lyö PUNAISELLE pallolle lisää vauhtia.
     * (vinkki: uudelleenkäytä Tehtava7-aliohjelmaa kutsumalla sitä näin "Tehtava7();")
     */
    public virtual void Tehtava9() { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Lisää ruudulle laskuri, joka pitää kirjaa siitä montako VALKOISTA
     *  palloa on vielä pelissä. Kun kaikki valkoiset pallot ovat kadonneet, lisää uusi
     *  satsi palloja käyttäen Tehtava5()-aliohjelmaa.
     */
    public virtual void Tehtava10() { throw new NotImplementedException(); }
}
