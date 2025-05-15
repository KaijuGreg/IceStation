using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefvsValue : MonoBehaviour {

    private void Start() {

        
        MyClass first = new MyClass(7);
        MyClass second = first; // this IMPORTANTLY here assigns a REFERENCE to the original data
        second.value = 5;
        Debug.Log("First Class Value: " + first.value); // both of the variables are referencing the same underlying class which is a reference type.
        // by modifying either variable you are modifying the ref for both, because Class is a ref type


        MyStruct firstStruct = new MyStruct(7);
        MyStruct secondStruct = firstStruct;
        secondStruct.value = 5;
        Debug.Log ("First Struct Value: " + firstStruct.value);


        int a = 7;
        int b = a; // this assigns a COPY, to the original 'int a' data
        b = 5;

        Debug.Log("value type int: a = " + a);
    }




}

public class MyClass {

    public int value;
    public MyClass( int value) {

        this.value = value;
    }


}

public struct MyStruct {

    public int value;
    public MyStruct(int value) {

        this.value = value;
    }


}
