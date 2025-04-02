/*
 * Copyright 2025 Scott Do
 * Defines the interface to save and load data.
 */
public interface IDataService
{
    // save data to a file
    bool SaveData<T> (string relativePath, T data);

    // load data from a file
    T LoadData<T>(string relativePath);
}
