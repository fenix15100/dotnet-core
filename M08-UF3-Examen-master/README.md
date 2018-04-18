# Examen M08-UF3

1.- Descarrega el projecte de C# des de aqui. Obre el projecte amb rider.

2.- Inicialitza el servidor LDAP/DNS.

## Part 1 LDAP

1.- (1 punt) Crea un usuari del tipus posixAccount amb l’inicial del teu nom i cognom (Ex: amilian), fixat en altres usuaris ja afegits. Pots fer-ho des de phpldapadmin del servidor amb el password ‘badia’.

2.- (2 punts) Implementa el mètode IsValidUser del Controlador AccountController per poder fer login amb l’usuari que has creat al punt 1.

3.- (3 punts) En la vista Home-Users llista tots els usuaris (amb el seu nom, cognom i DN) que hi ha en el directori LDAP.

## Part 2 DNS

4.-(1 punt) Crear el FQDN ldap.examen.ies que redireccioni cap a l'adreça IP asignada per DHCP del mateix servidor, al fitexer examen.zone.

5.-(1 punt) Crear l'alias directori.examen.ies de ldap.examen.ies.

6.-(1 punt) Crea un nou domini amb el nom aprovat.ies i que existeixi el FQDN search.aprovat.ies apuntant a l’adreça IP 216.58.211.227. 

7.-(1 punt) Configura el servidor DNS perquè també pugui resoldre adreces que no coneix, delegant al servidor DNS 8.8.8.8. 

###Notes:
- Tots els dominis es validaran amb la comanda dig @127.0.0.1 \<domini\>
- Per reiniciar el servei bind s’ha de fer servir __service bind9 restart__.
