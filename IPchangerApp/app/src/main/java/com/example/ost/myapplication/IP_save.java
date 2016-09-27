package com.example.ost.myapplication;

import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

/**
 * Created by OST on 2016-09-26.
 */
public class IP_save extends SQLiteOpenHelper {

    public IP_save(Context context, String name, SQLiteDatabase.CursorFactory factory, int version) {
        super(context, name, factory, version);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        // 새로운 테이블을 생성한다.
        // create table 테이블명 (컬럼명 타입 옵션);
        db.execSQL("CREATE TABLE IP_ADDRESS( _id INTEGER PRIMARY KEY, ip_addr TEXT);");

        db.execSQL("insert into IP_ADDRESS values(1, \"192.168.43.\")");

    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
    }

    public String PrintData() {

        SQLiteDatabase db = this.getWritableDatabase();
        String str = "";


        Cursor cursor = db.rawQuery("select * from IP_ADDRESS where _id = 1", null);

        cursor.moveToFirst();
        str = cursor.getString(1);

        return str;
    }

    public void update(String _query) {
        SQLiteDatabase db = getWritableDatabase();
        db.execSQL(_query);
    }

}
