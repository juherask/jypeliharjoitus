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
    List<GameObject> objektitEnnenTehtavaa = null;
    List<GameObject> talletetutObjektitTehtava3 = null;
    List<GameObject> talletetutObjektitTehtava4 = null;

    int objektienLukumaaraTehtava4 = 0;
    int tilanTarkistusLaskuriTehtava7 = 0;
    int tilanTarkistusLaskuriTehtava8 = 0;
    PhysicsObject palloLiikkeellaTehtava8 = null;
    Vector pallonAlkuvauhtiTehtava8 = new Vector();
    int tilanTarkistusLaskuriTehtava9 = 0;
    int edellinenMaaraPallojaTehtava9 = -1;
    int tilanTarkistusLaskuriTehtava10 = 0;
    Color taustavariTehtava10 = Color.SkyBlue;

    public override void Begin()
    {
        SetWindowSize(1280, 720);

        Animation pacanim = new Animation(LoadImages("pac1", "pac2"));
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

            GameObject kolikko = new GameObject(40, 40);
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
        if (tehtava > 0 && tehtava<11)
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
                tulos = TarkistaTehtava4();
                break;
            case 5:
                tulos = TarkistaTehtava5();
                break;
            case 6:
                tulos = TarkistaTehtava6();
                break;
            case 7:
                tulos = TarkistaTehtava7();
                break;
            case 8:
                tulos = TarkistaTehtava8();
                break;
            case 9:
                tulos = TarkistaTehtava9();
                break;
            case 10:
                tulos = TarkistaTehtava10();

                if (tulos == TehtavanTila.ToteutettuToimii)
                {
                    var voitto = new GameObject(Screen.Width, Screen.Height);
                    voitto.Image = LoadImage("ikplus_diploma");
                    Add(voitto, 3);
                    var naytto = GetObjects(g => g is Label).First();
                    naytto.Destroy();

                    MediaPlayer.Play("fanfare");
                }
                break;
            default:
                tulos = TehtavanTila.ToteutettuToimii;
                break;
        }

        // Toimi tuloksen mukaisesti
        if (tulos == TehtavanTila.ToteutettuToimii)
        {
            if (tehtava > 0 && tehtava < 11)
                MessageDisplay.Add(String.Format("Tehtava{0} läpi!", tehtava));
            double seuraavaanTarkastukseen = PoistaKolikko();
            tehtava++;
            Timer.SingleShot(seuraavaanTarkastukseen, TarkistaTehtava);
        }
        else if (tulos == TehtavanTila.ToteutettuEiToimi)
        {
            PoistaKolikko();

            // Pistä haamut ajamaan Pacmaniä takaa
            Vector ulosVasemmalta = new Vector(Screen.Left - haamut.Width / 2, pacman.Y);
            Animation peruutusAnimaatio = Animation.Mirror(new Animation(LoadImages("pac1", "pac2")));
            pacman.Animation = peruutusAnimaatio;
            pacman.Animation.FPS = 5;
            pacman.Animation.Start();
            pacman.MoveTo(ulosVasemmalta, PACSPEED);
            haamut.MoveTo(ulosVasemmalta, PACSPEED);
        }
        else if (tulos == TehtavanTila.ToteutettuSaattaaToimia)
        {
            Timer.SingleShot(0.02, TarkistaTehtava);
        }

    }

    private TehtavanTila TarkistaTehtava10()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            tila = TehtavanTila.ToteutettuSaattaaToimia;
            var naytto = GetObjects(g => g is Label).First();

            if (tilanTarkistusLaskuriTehtava10 == 0)
            {
                Tehtava10();

                taustavariTehtava10 = Level.Background.Color;

                if (taustavariTehtava10 == Color.Pink)
                {
                    MessageDisplay.Add("Tehtävä10: Taustaväri on jo valmiiksi vaaleanpunainen.");
                    tila = TehtavanTila.ToteutettuEiToimi;
                }
            }
            else
            {
                int valkoisiaPalloja = GetObjects(g => g.Color == Color.White && g.Shape == Shape.Circle).Count;

                if (valkoisiaPalloja < objektienLukumaaraTehtava4 * 0.5)
                {
                    if (taustavariTehtava10 != Level.Background.Color)
                    {
                        if (Level.Background.Color != Color.Pink)
                        {
                            MessageDisplay.Add("Tehtävä10: Taustaväri muuttui, mutta ei vaaleanpunaiseksi.");
                            tila = TehtavanTila.ToteutettuEiToimi;
                        }
                        else
                        {
                            tila = TehtavanTila.ToteutettuToimii;
                        }
                    }
                    else
                    {
                        MessageDisplay.Add("Tehtävä10: Palloja alle puolet jäljellä eikä taustaväri muuttunut (tai sitten JyPeli bugaa, kokeile uudelleen).");
                        tila = TehtavanTila.ToteutettuEiToimi;
                    }
                }
                else
                {
                    if (taustavariTehtava10 != Level.Background.Color)
                    {
                        MessageDisplay.Add("Tehtävä10: Taustaväri muuttui ennen aikojaan (tai sitten JyPeli bugaa, kokeile uudelleen).");
                        tila = TehtavanTila.ToteutettuEiToimi;
                    }
                }
            }

            tilanTarkistusLaskuriTehtava10++;

        }
        catch (NotImplementedException)
        {
            tila = TehtavanTila.EiToteutettu;
        }
        return tila;
    }


    private TehtavanTila TarkistaTehtava9()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            tila = TehtavanTila.ToteutettuEiToimi;

            var naytot = GetObjects(g => g is Label);

            if (tilanTarkistusLaskuriTehtava9 == 0)
            {
                // Kutsutaan aliohjelmaa, joka lisää laskurin
                if (naytot.Count > 0)
                {
                    MessageDisplay.Add("Tehtävä9: Olet lisännyt laskurin näytön liian varhain. Lisää se vasta harjoitus-aliohjelmassa.");
                }
                else
                {
                    int valkoisiaPalloja = GetObjects(g => g.Color == Color.White && g.Shape == Shape.Circle).Count;
                    Tehtava9(valkoisiaPalloja);
                    edellinenMaaraPallojaTehtava9 = valkoisiaPalloja;
                    tila = TehtavanTila.ToteutettuSaattaaToimia;
                }
            }
            else if (tilanTarkistusLaskuriTehtava9 == 1)
            {
                // Tarkistetaan, että laskurin määrä vastaa pallojen lkm

                if (naytot.Count == 1)
                {
                    Label naytto = naytot.First() as Label;
                    int valkoisiaPalloja = GetObjects(g => g.Color == Color.White && g.Shape == Shape.Circle).Count;
                    int naytonPalloja = Int32.Parse(naytto.Text.Split().Last());

                    if (valkoisiaPalloja != naytonPalloja && edellinenMaaraPallojaTehtava9 == naytonPalloja)
                    {
                        MessageDisplay.Add("Tehtävä9: Näyttö ei seuraa pallojen lukumäärää.");
                    }
                    else
                    {
                        edellinenMaaraPallojaTehtava9 = valkoisiaPalloja;
                        tila = TehtavanTila.ToteutettuSaattaaToimia;
                    }
                }
                else
                {
                    MessageDisplay.Add("Tehtävä9: Aliohjelma ei lisää peliin lukumäärän näyttävää näyttöä (Label).");
                }
            }
            else if (tilanTarkistusLaskuriTehtava9 < 500)
            {
                Label naytto = naytot.First() as Label;
                int valkoisiaPalloja = GetObjects(g => g.Color == Color.White && g.Shape == Shape.Circle).Count;
                int naytonPalloja = Int32.Parse(naytto.Text.Split().Last());

                if (edellinenMaaraPallojaTehtava9 == valkoisiaPalloja)
                {
                    tila = TehtavanTila.ToteutettuSaattaaToimia;
                }
                else
                {
                    if (valkoisiaPalloja == naytonPalloja)
                    {
                        // Meille riittää siis, että toteamme kerran arvon pienentyvän 
                        tila = TehtavanTila.ToteutettuToimii;
                    }
                    else
                    {
                        MessageDisplay.Add("Tehtävä9: Pallojen määrä hupenee, mutta laskurinäyttö ei pienene tai näytä oikein (tai sitten JyPeli bugaa, kokeile uudestaan).");
                    }
                }
            }
            else
            {
                MessageDisplay.Add("Tehtävä9: Pallot eivät näytä törmäilevän, eikä laskurinäyttö siksi päivity.");
            }

            tilanTarkistusLaskuriTehtava9++;


        }
        catch (NotImplementedException)
        {
            tila = TehtavanTila.EiToteutettu;
        }
        return tila;
    }

    private TehtavanTila TarkistaTehtava8()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            tila = TehtavanTila.ToteutettuEiToimi;
            if (tilanTarkistusLaskuriTehtava8 == 0)
            {
                tilanTarkistusLaskuriTehtava8++;
                if (talletetutObjektitTehtava3 != null && talletetutObjektitTehtava3.Count != 0)
                {
                    palloLiikkeellaTehtava8 = talletetutObjektitTehtava3[0] as PhysicsObject;
                    pallonAlkuvauhtiTehtava8 = palloLiikkeellaTehtava8.Velocity;
                    Tehtava8();

                    tila = TehtavanTila.ToteutettuSaattaaToimia;
                }
                else
                {
                    MessageDisplay.Add("Tehtävä8: Ohjelmassa on jotakin vikaa, yritä käynnistää se uudestaan");
                    //return tila;
                }
            }
            else if (palloLiikkeellaTehtava8.Velocity.Magnitude <= pallonAlkuvauhtiTehtava8.Magnitude && tilanTarkistusLaskuriTehtava8 < 500)
            {
                if (tilanTarkistusLaskuriTehtava8 == 1)
                {
                    MessageDisplay.Add("Tehtävä8: Anna pallolle vauhtia painamalla VÄLILYÖNTIÄ.");
                }
                tilanTarkistusLaskuriTehtava8++;
                tila = TehtavanTila.ToteutettuSaattaaToimia;
            }
            else if (tilanTarkistusLaskuriTehtava8 >= 500)
            {
                MessageDisplay.Add("Tehtävä8: Pallo ei saanut lisää vauhtia, annoitko sille varmasti uuden iskun?");
            }
            else if (palloLiikkeellaTehtava8.Velocity.Magnitude > pallonAlkuvauhtiTehtava8.Magnitude)
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

    private TehtavanTila TarkistaTehtava7()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            if (objektitEnnenTehtavaa == null)
            {
                // Precondition
                objektitEnnenTehtavaa = GetObjects(go => go is PhysicsObject);
                Tehtava7(talletetutObjektitTehtava4.Cast<PhysicsObject>().ToList());
                // Tehtavan kutsumisen jälkeen pitää antaa JyPelille aikaa tehdä työnsä.
                //  tähän tarkistajaan tullaan siis myöhemmin uudelleen, mutta tätä haaraa ei enää tehdä.
                tila = TehtavanTila.ToteutettuSaattaaToimia;

                return tila;
            }

            tila = TehtavanTila.ToteutettuEiToimi;
            tilanTarkistusLaskuriTehtava7++;
            List<GameObject> objektitTormayksenJalkeen = GetObjects(go => go is PhysicsObject);

            //string maara = string.Format("objektit {0}", objektitTormayksenJalkeen.Count);
            //MessageDisplay.Add(maara);

            if (objektitTormayksenJalkeen.Count >= talletetutObjektitTehtava4.Count && tilanTarkistusLaskuriTehtava7 < 700)
            {
                tila = TehtavanTila.ToteutettuSaattaaToimia;
            }
            else if (tilanTarkistusLaskuriTehtava7 >= 700)
            {
                MessageDisplay.Add("Tehtävä7: Pallot eivät tuhoudu törmätessä tai eivät törmäile");
            }
            else if (objektitTormayksenJalkeen.Count < talletetutObjektitTehtava4.Count)
            {
                tila = TehtavanTila.ToteutettuToimii;
                objektitEnnenTehtavaa.Clear();
                objektitEnnenTehtavaa = null;
                //MessageDisplay.Add("Tehtävä 7 on toteutettu ja toimii");
            }
        }
        catch (NotImplementedException)
        {
            tila = TehtavanTila.EiToteutettu;
        }
        return tila;
    }

    private TehtavanTila TarkistaTehtava6()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            PhysicsObject palloLiikkeella = null;
            Tehtava6();

            tila = TehtavanTila.ToteutettuEiToimi;
            if (talletetutObjektitTehtava3 != null && talletetutObjektitTehtava3.Count != 0)
            {
                palloLiikkeella = talletetutObjektitTehtava3[0] as PhysicsObject;
            }
            else
            {
                MessageDisplay.Add("Tehtävä6: Ohjelmassa on jotakin vikaa, yritä käynnisttää se uudestaan");
                return tila;
            }

            if (palloLiikkeella.Velocity == Vector.Zero)
            {
                MessageDisplay.Add("Tehtävä6: Pallo ei liiku, annoitko sille varmasti iskun?");
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

    private TehtavanTila TarkistaTehtava5()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            if (objektitEnnenTehtavaa == null)
            {
                // Precondition
                objektitEnnenTehtavaa = GetObjects(go => go is PhysicsObject);
                Tehtava5();
                // Tehtavan kutsumisen jälkeen pitää antaa JyPelille aikaa tehdä työnsä.
                //  tähän tarkistajaan tullaan siis myöhemmin uudelleen, mutta tätä haaraa ei enää tehdä.
                tila = TehtavanTila.ToteutettuSaattaaToimia;
                return tila;
            }

            List<GameObject> uudet = GetObjects(go => go is PhysicsObject && !objektitEnnenTehtavaa.Contains(go));

            tila = TehtavanTila.ToteutettuEiToimi;
            if (uudet.Count < 4)
            {
                MessageDisplay.Add("Tehtävä5: Et ole lisännyt kaikkia neljää reunaa");
            }
            else if (uudet.Count > 4)
            {
                MessageDisplay.Add("Tehtävä5: Olet lisännyt liian monta pelioliota");
            }
            else if (!TarkistaOlioidenMuoto(Shape.Rectangle, uudet))
            {
                MessageDisplay.Add("Tehtävä5: Reunat ovat väärän muotoiset");
            }
            else if (!TarkistaReunat(uudet))
            {
                MessageDisplay.Add("Tehtävä5: Reunat ovat väärän kokoiset tai väärässä paikassa");
            }
            else
            {
                tila = TehtavanTila.ToteutettuToimii;
                objektitEnnenTehtavaa.Clear();
                objektitEnnenTehtavaa = null;
            }
        }
        catch (NotImplementedException)
        {
            tila = TehtavanTila.EiToteutettu;
        }
        return tila;
    }

    private TehtavanTila TarkistaTehtava4()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            if (objektitEnnenTehtavaa == null)
            {
                objektienLukumaaraTehtava4 = RandomGen.NextInt(100, 150);
                // Precondition
                objektitEnnenTehtavaa = GetObjects(go => go is PhysicsObject);
                Tehtava4(objektienLukumaaraTehtava4);
                // Tehtavan kutsumisen jälkeen pitää antaa JyPelille aikaa tehdä työnsä.
                //  tähän tarkistajaan tullaan siis myöhemmin uudelleen, mutta tätä haaraa ei enää tehdä.
                tila = TehtavanTila.ToteutettuSaattaaToimia;
                return tila;
            }

            talletetutObjektitTehtava4 = GetObjects(go => go is PhysicsObject && !objektitEnnenTehtavaa.Contains(go));

            tila = TehtavanTila.ToteutettuEiToimi;
            if (talletetutObjektitTehtava4.Count < objektienLukumaaraTehtava4)
            {
                MessageDisplay.Add("Tehtävä4: Peliolioita ei ole lisätty tarpeeksi monta");
            }
            else if (talletetutObjektitTehtava4.Count > objektienLukumaaraTehtava4)
            {
                MessageDisplay.Add("Tehtävä4: Liian monta uutta pelioliota lisätty");
            }
            else if (!TarkistaOlioidenMuoto(Shape.Circle, talletetutObjektitTehtava4))
            {
                MessageDisplay.Add("Tehtävä4: Lisäämäsi pallo ei ole pyöreä");
            }
            else if (!TarkistaOlioidenVari(Color.White, talletetutObjektitTehtava4))
            {
                MessageDisplay.Add("Tehtävä4: Pallot eivät ole valkoisia!");
            }
            else if (talletetutObjektitTehtava4[0].Position == new Vector(0, 0))
            {
                MessageDisplay.Add("Tehtävä4: Pallosi on keskellä ruutua. Oletko varma, että laitoit sen satunnaiseen paikkaan? Yritä uudestaan");
            }
            else if (!TarkistaOlioidenEtaisyys(300, talletetutObjektitTehtava4))
            {
                MessageDisplay.Add("Tehtävä4: Pallosi on liian kaukana keskipisteestä. Yritäppä uudestaan varmuuden vuoksi");
            }
            else
            {
                tila = TehtavanTila.ToteutettuToimii;
                objektitEnnenTehtavaa.Clear();
                objektitEnnenTehtavaa = null;
            }
        }
        catch (NotImplementedException)
        {
            tila = TehtavanTila.EiToteutettu;
        }
        return tila;
    }

    private TehtavanTila TarkistaTehtava3()
    {
        TehtavanTila tila = TehtavanTila.EiToteutettu;
        try
        {
            if (objektitEnnenTehtavaa == null)
            {
                // Precondition
                objektitEnnenTehtavaa = GetObjects(go => go is PhysicsObject);
                Tehtava3();
                // Tehtavan kutsumisen jälkeen pitää antaa JyPelille aikaa tehdä työnsä.
                //  tähän tarkistajaan tullaan siis myöhemmin uudelleen, mutta tätä haaraa ei enää tehdä.
                tila = TehtavanTila.ToteutettuSaattaaToimia;
                return tila;
            }

            List<GameObject> uudet = GetObjects(go => go is PhysicsObject && !objektitEnnenTehtavaa.Contains(go));
            talletetutObjektitTehtava3 = uudet;

            tila = TehtavanTila.ToteutettuEiToimi;
            if (uudet.Count == 0)
            {
                MessageDisplay.Add("Tehtävä3: Punaista palloa edustavaa pelioliota ei ole lisätty");
            }
            else if (uudet.Count > 1)
            {
                MessageDisplay.Add("Tehtävä3: Liian monta uutta pelioliota lisätty");
            }
            else if (uudet[0].Shape != Shape.Circle)
            {
                MessageDisplay.Add("Tehtävä3: Lisäämäsi pallo ei ole pyöreä");
            }
            else if (uudet[0].Color != Color.Red)
            {
                MessageDisplay.Add("Tehtävä3: Pallosi ei ole punainen, muistitko vaihtaa värin oikein!");
            }
            else
            {
                tila = TehtavanTila.ToteutettuToimii;
                objektitEnnenTehtavaa.Clear();
                objektitEnnenTehtavaa = null;
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
                    string virhe = String.Format(@"Tehtävä2: Koodi ei laske oikein keskiarvoa. Esim. koodi antaa
{0:F2}, {1:F2}, {2:F2} keskiarvoksi {3:F2} kun sen pitäisi olla {4:F2}", x, y, z, Tehtava2(x, y, z), (x + y + z) / 3);
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
                    string virhe = String.Format("Tehtävä1: Koodi ei laske oikein lukuja yhteen. Esim. {0}+{1}={2}, kun pitäisi olla {3}", a, b, Tehtava1(a, b), a + b);
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

    bool TarkistaOlioidenMuoto(Shape muoto, List<GameObject> oliot)
    {
        for (int i = 0; i < oliot.Count; i++)
        {
            if (oliot[i].Shape != muoto)
            {
                return false;
            }
        }

        return true;
    }

    bool TarkistaOlioidenVari(Color vari, List<GameObject> oliot)
    {
        for (int i = 0; i < oliot.Count; i++)
        {
            if (oliot[i].Color != vari)
            {
                return false;
            }
        }

        return true;
    }

    bool TarkistaOlioidenEtaisyys(int maxEtaisyys, List<GameObject> oliot)
    {
        for (int i = 0; i < oliot.Count; i++)
        {
            if (oliot[i].X > maxEtaisyys || oliot[i].Y > maxEtaisyys)
            {
                return false;
            }
        }

        return true;
    }

    bool TarkistaReunat(List<GameObject> oliot)
    {
        bool returnValue = true;

        for (int i = 0; i < oliot.Count; i++)
        {
            // Tarkista on vasen tai oikea reuna
            if (oliot[i].Y == 0)
            {
                // Tarkista koko
                if (oliot[i].Width != Level.Height)
                {
                    returnValue = false;
                }
                // Tarista paikka
                else if ((Math.Abs(oliot[i].X) - (oliot[i].Height / 2)) != (Level.Width / 2))
                {
                    returnValue = false;
                }
            }
            // Tarkista onko ylä tai ala reuna
            else if (oliot[i].X == 0)
            {
                // Tarkista koko
                if (oliot[i].Width != (Level.Width + (oliot[i].Height * 2)))
                {
                    returnValue = false;
                }
                // Tarkista paikka
                else if ((Math.Abs(oliot[i].Y) - (oliot[i].Height / 2)) != (Level.Height / 2))
                {
                    returnValue = false;
                }
            }
        }

        return returnValue;
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
     * Tehtävänanto: Lisää PUNAINEN pallo peliin
     */
    public virtual void Tehtava3() { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Lisää peliin n kpl satunnaisessa paikassa olevaa VALKOISTA palloa
     * korkeintaan 300.0 päähän ruudun keskipisteestä
     * Vector paikka = RandomGen.NextVector(0.0, 300.0);
     * (vinkki, käytä for-silmukkaa)
     */
    public virtual void Tehtava4(int n) { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Lisää peliin reunat joka puolelle
     */
    public virtual void Tehtava5() { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Lyö PUNAISELLE pallolle vauhtia satunnaiseen suuntaan
     *  (vinkki, tee Tehtava4-aliohjelmassa lisättävästä pallosta luokkamuuttuja) 
     */
    public virtual void Tehtava6() { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Kun kaksi VALKOISTA palloa osuu toisiinsa, pistä ne katoamaan
     *  lisäpisteitä jos saat ne räjähtämään (kersku siitä kaverille ja opettajille :)
     *  (vinkki, tarvitset törmäyskäsittelijää ja uuden aliohjelman)
     */
    public virtual void Tehtava7(List<PhysicsObject> pallot) { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Aina kun välilyöntiä painetaan, lyö PUNAISELLE pallolle lisää vauhtia.
     * (vinkki: uudelleenkäytä Tehtava6-aliohjelmaa kutsumalla sitä näin "Tehtava6();")
     */
    public virtual void Tehtava8() { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Lisää ruudulla näkyvä laskuri, joka pitää kirjaa siitä montako VALKOISTA
     *  palloa on vielä pelissä.
     */
    public virtual void Tehtava9(int pallojaTallaHetkella) { throw new NotImplementedException(); }

    /*
     * Tehtävänanto: Kun jäljellä on enää alle puolet VALKOISISTA palloista, vaihda kentän 
        //  taustaväri vaaleanpunaiseksi (Color.Pink).
     */
    public virtual void Tehtava10() { throw new NotImplementedException(); }
}
