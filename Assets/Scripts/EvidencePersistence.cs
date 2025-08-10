using UnityEngine;

public enum EvidenceKind { Cinta, Carta, Fotografia }

public class EvidencePersistence : MonoBehaviour
{
    [SerializeField] private EvidenceKind kind;

    void Start()
    {
        if (YaFueDestruida(kind))
            gameObject.SetActive(false);
    }

    bool YaFueDestruida(EvidenceKind k)
    {
        switch (k)
        {
            case EvidenceKind.Cinta: return HistoriaProgreso.cintaReproducida;
            case EvidenceKind.Carta: return HistoriaProgreso.cartaDestruida;
            case EvidenceKind.Fotografia: return HistoriaProgreso.fotografiaDestruida;
        }
        return false;
    }
}
