using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strings{


    public static StringSets Heart() {
        StringSets heart;

        heart.title = "The Heart";
        heart.description = "An extra heart for those who have lost theirs...";
        heart.price = 300;

        return heart;

    }
    public static StringSets Sheild() {
        StringSets sheild;

        sheild.title = "Sheild";
        sheild.description = "An invincible sheild that will protect anyone even from sheilds";
        sheild.price = 100;

        return sheild;
    }

    public static StringSets Phone() {
        StringSets phone;

        phone.title = "Mighty Phone";
        phone.description = "With the mighty phone, one can call friends for reinforcements";
        phone.price = 300;

        return phone;
    }

    public static StringSets Magnet() {
        StringSets magnet;

        magnet.title = "Magnet";
        magnet.description = "A gigantic magnet that attracts garbage. Use it wisely";
        magnet.price = 50;

        return magnet;
    }

    public static StringSets Sweeper() {
        StringSets sweeper;

        sweeper.title = "The Sweeper";
        sweeper.description = "A truck designed only for one purpose only, to CLEAN!";
        sweeper.price = 600;

        return sweeper;
    }

    public static StringSets Bomb() {
        StringSets bomb;

        bomb.title = "The RRR";
        bomb.description = "The ultimate weapon against pollution, don't waste it";
        bomb.price = 900;

        return bomb;
    }

    public static StringSets GearsOfWar() {
        StringSets gears;

        gears.title = "Gears of war";
        gears.description = "A powerful tech designed to clean up the sea of garbage";
        gears.price = 850;

        return gears;
    }

    public static StringSets GetStringSet(string index) {
        switch (index) {
            case "Heart":
                return Heart();
            case "Sheild":
                return Phone();
            case "Phone":
                return Sheild();
            case "Magnet":
                return Magnet();
            case "Sweeper":
                return Sweeper();
            case "Bomb":
                return Bomb();
            case "GearsOfWar":
                return GearsOfWar();
            default:
                return new StringSets();
        }
    }

    public static StringSets GetStringSet(int index) {
        switch (index) {
            case 0:
                return Heart();
            case 1:
                return Phone();
            case 2:
                return Sheild();
            case 3:
                return Magnet();
            case 4:
                return Sweeper();
            case 5:
                return Bomb();
            case 6:
                return GearsOfWar();
            default:
                return new StringSets();
        }
    }



}


public struct StringSets {

    public string title;
    public string description;
    public int price;

}