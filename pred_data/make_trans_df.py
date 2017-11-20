import numpy as np
import pandas as pd
from sklearn.decomposition import PCA
from sklearn.feature_extraction import DictVectorizer
from sklearn.tree import DecisionTreeClassifier
from sklearn.ensemble import RandomForestClassifier
import warnings


def is_int(x):
  try:
    int(x)
    return True
  except ValueError:
    return False


def parse_features(filename):
  # Read in features from PDF's CSV and remove malformed
  features = pd.read_csv(filename)
  features = features.dropna(subset=['ITEM'])
  features = features[features['ITEM'].apply(lambda x: is_int(x))]

  features = features.rename(columns={
    'ITEM': 'ITEM NO.',
    'FIELD': 'FIELD LENGTH',
    'FILE': 'FILE LOCATION',
    'Unnamed: 3': '[ITEM NAME], DESCRIPTION, AND CODES'
    })

  # Find data errors in features
  for i in range(1, 1067):
    count = len(features[features['ITEM NO.'] == str(i)])
    if count != 1:
      w = str(count) + ' items numbered "' + str(i) + '" instead of 1.'
      warnings.warn(w, Warning)

  # Find data errors in features
  if len(features.isnull().any(1).nonzero()[0]) > 0:
    w = 'nan values at printed indices'
    print(features.isnull().any(1).nonzero())
    warnings.warn(w, Warning)
    print(features.iloc[features.isnull().any(1).nonzero()[0][0]])

  return features


def create_row(features_loc, line):
  new_row = []
  for col_idx, input_idx in enumerate(features_loc):
    if '-' in input_idx:
      split = input_idx.split('-')
      new_row.append(line[int(split[0]) - 1 : int(split[1])])
    else:
      new_row.append(line[int(input_idx) - 1])

  return new_row


def create_df(filename, features):
  features_loc = features['FILE LOCATION']
  features_name = features['[ITEM NAME], DESCRIPTION, AND CODES']
  
  f = open(filename, 'r')
  lines = f.readlines()

  data = []
  for row_idx, line in enumerate(lines):
    new_row = create_row(features_loc, line)
    data.append(new_row)

  df = pd.DataFrame(data, columns=features_name)
  return df


def convert_floats_where_possible(df):
  N_UNI = 10
  print('shape init:', df.shape)
  for col in df.columns:
    if len(df[col].unique()) == 1:
      print('  lonely col', col)
      df = df[df.columns.difference([col])]
      continue
    if df[col].nunique() > N_UNI:
      try:
        df[col] = [float(element) for element in df[col]]
        print('', end='')
      except:
        if df[col].nunique() > (200):
          print('  crowded col(' + str(df[col].nunique()) + ')', col)
          df = df[df.columns.difference([col])]
        print('', end='')
    else:
      print('', end='')

  print('shape  fin:', df.shape)
  return df


def transform_df(df):
  print('creating dictionary...')
  df = df.to_dict('records')
  print('dictionary complete')

  vec = DictVectorizer()
  print('transforming df...')
  return pd.DataFrame(data=vec.fit_transform(df).toarray(), columns=vec.get_feature_names())


def data_cleanse_helper(df):
  features = parse_features('tabula_desc.csv')
  df = create_df('namcs2015', features)
  return df


def transform_helper(df):
  df = convert_floats_where_possible(df)
  return transform_df(df)


def try_file(name, helper, df=None):
  try:
    print('loading ' + name + '...')
    df = pd.read_pickle(name)
    print('load complete')
  except:
    print('load failed')
    print('creating ' + name + '...')
    df = helper(df)
    print('saving ' + name + '...')
    df.to_pickle(name)
    print('creation/save complete')

  return df


def main():
  try:
    print('loading X, y reduced')
    X = np.load('X_red.npy')
    y = np.load('y_red.npy')
    print('load complete')
  except:
    print('load failed')
    try:
      df = try_file('transformed_df.pkl', transform_helper, df)
    except:
      df = try_file('namcs2015_parsed.pkl', data_cleanse_helper)
      df = try_file('transformed_df.pkl', transform_helper, df)

    print('transformation to:', df.shape)

    print('copying matrices...')
    X = df[df.columns.difference(['[DIAG1R] DIAGNOSIS # 1 (Recode to Numeric Field)'])] 
    y = df['[DIAG1R] DIAGNOSIS # 1 (Recode to Numeric Field)']
    del df
    print('copy complete')

    print('reducing model...')
    pca = PCA(n_components=100)
    X = pca.fit_transform(X)
    print('model reduced:', X.shape)

    print('saving reduction...')
    np.save('X_red', X)
    np.save('y_red', y)
    print('save complete')

  # X = X[0:100,:]
  # y = y[0:100]
  mask = np.random.rand(len(X)) < 0.95

  # print('classifying DecisionTreeClassifier..')
  # clf = DecisionTreeClassifier()
  # clf.fit(X[mask], y[mask])
  # print('classification complete')
  # print('score', clf.score(X[~mask], y[~mask]))

  # print('classifying DecisionTreeClassifier..')
  # clf = DecisionTreeClassifier()
  # clf.fit(X[mask], y[mask])
  # print('classification complete')
  # print('score', clf.score(X[~mask], y[~mask]))

  
  y_test = y[~mask]
  y_correct = [False] * len(y_test)
  for i in range(0,10):
    print('iteration:', i)
    clf = DecisionTreeClassifier()
    clf.fit(X[mask], y[mask])
    y_pred = clf.predict(X[~mask])
    for idx, j in enumerate(y_pred):
      if j == y_test[idx]:
        y_correct[idx] = True

  print('any correct:', y_correct.count(True) / len(y_correct))

  #print('classifying RandomForestClassifier..')
  #clf = RandomForestClassifier()
  #clf.fit(X[mask], y[mask])
  #print('classification complete')
  #print('score', clf.score(X[~mask], y[~mask]))
  

if __name__ == "__main__":
  main()
