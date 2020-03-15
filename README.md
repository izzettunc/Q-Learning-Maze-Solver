# Programın tüm ayarları SampleScene in içinde ki Handler nesnesi üzerinde yapılmaktadır.

# Q Table epoch sınırına ulaştığında bastırılacaktır.

## Maze Length

Sınırları 2<=X<=N şeklindedir.

## Select Random Start And End

Standart halinde başlangıç ve bitiş noktaları sırasıyla satır ve sutun olmak üzere 1,1 ve N,N de
bulunmaktadır.Seçim yapıldığından başlangıç ve bitiş noktasını rastgele yerlere yerleştirir.

## Step Per Second

Bir saniye de kaç adım çalışması gerektiğini belirtir.
Sınırları 0<X<=MAX FPS şeklindedir.

## Waiting Time At The End

Hedefe ulaştığında bir sonraki epoch a geçmeden önce kaç saniye beklemesi gerektiğini belirtir.
Sınırları 0<=X<=N şeklindedir.

## Reward

Q Tablosunda ki ödül değeri kaç olmalı.
Sınırları 0<X<=N şeklindedir.

## Learning Rate

## Discount Factor

## Epoch

Kaç epoch (Kaç kere bitiş noktasına ulaşacak) çalışması gerektiğini belirtir.Sonsuz çalışması için -1 giriniz.

## Stop Learning At

Kaç epoch sonunda öğrenmeyi bırakması gerektiğini belirtir.Sonsuz öğrenme için -1 giriniz.

## Smart Choosing

Algoritmayı yanlış anlamam sonucunda ortaya çıkan bir adım fazla ileriye bakan method.Silmeye kıyamadım.
