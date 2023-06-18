Bu projede Warehouse Location Problem(Depo Yerleşim Problemi)'nin Tabu Arama algoritma ile çözümü ele alınmıştır.
Tabu arama algoritması, verilen tabu boyutunda (genetik algoritmadaki popülasyon gibi düşünebilirsiniz.)
verilen iterasyon sayısı kadar döner ve en optimal sonucu bulmaya çalışır. Burada iterasyon sayısını 
yüksek tutmak bizim için global optimum çözüm açısından faydalı olacaktır çünkü her bir iterasyonda
global optimal çözüme bir adım daha yakınsar ancak madalyonun diğer yüzünden bakıcak olursak
bizim için memory ve zaman açısından da maliyetli olacaktır, bunun için verilen dosya boyutuna 
göre(eğer düşük boyutlu ise iterasyon yüksek, yüksek boyutlu ise iterasyon düşük) uygun
bir iterasyon sayısı verilmesi gerekir.

Gereksinim Dökümanı: [ödev3_gereksinim_dokumanı 1 (2).pdf](https://github.com/osman28tr/Warehouse-Location-Problem-Solution-With-TabuSearch/files/11781463/odev3_gereksinim_dokumani.1.2.pdf)

Kullanılan Dil: C#

Kullanılan girdi dosyalarına bin->debug klasörü altından erişebilirsiniz.(wl_16_1.txt,wl_200_2.txt,wl_500_3.txt)
