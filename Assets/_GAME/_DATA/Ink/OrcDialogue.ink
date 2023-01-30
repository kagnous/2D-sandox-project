VAR extorted = false

-> Start

=== Start ===
Tiens tiens, voilà quelqu'un...

Salut, l'humain
    +[Bonjours...]
        Quoi ! Tu ose me répondre, misérable !!! #portrait:angry
        ->END
        
    +[La bouse ou la vie, saleté d'engeance !]
        HA HA HA !!! #portrait:happy
        Qu'est-ce tu viens de dire, le microbe ?
        ++[J'ai dis, file moi toute ta thune, petite chiure d'orc verdatre, où je t'arrache les crocs]
                {extorted == false :
                    Ok ok, pas la peine de s'énerver, voilà pour toi...#Add:gold #portrait:sad
                    ~ extorted = true
                -else:
                    Encore ! Hé oh faut pas abuser là ! #portrait:angry
                }
                -> END
        
        ++[Pardon monsieur... OwO]
        <I>Vous partez en vitesse, honteux </I>
        -> END
    +[Salut, et au revoir (vous fuyez)]
        C'est ça, cours, la lopette ! #portrait:happy
        -> END