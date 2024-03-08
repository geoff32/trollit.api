namespace TrollIt.Infrastructure.Profiles.Scripts.Models;

public record Situation(
    int X,
    int Y,
    int N,
    double Dla,
    int Pa,
    Fatigue Fatigue,
    int NbEsq,
    bool Camouflage,
    bool Invisible,
    bool Intangible,
    int NbCA,
    int NbParades,
    int nbTouche,
    bool Glue,
    bool ATerre,
    int Course,
    int Levitation,
    string DirRetraites
);